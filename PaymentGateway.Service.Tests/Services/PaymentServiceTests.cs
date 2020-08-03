using AutoMapper;
using Moq;
using NUnit.Framework;
using PaymentGateway.Core.Enums;
using PaymentGateway.Core.Helpers;
using PaymentGateway.Core.Models;
using PaymentGateway.Data.Entities;
using PaymentGateway.Data.Repositories;
using PaymentGateway.Service.Clients;
using PaymentGateway.Service.Services;
using PaymentGateway.Service.Validators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Tests.Services
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private Mock<IPaymentRepository> _paymentRepositoryMock;
        private Mock<IMerchantRepository> _merchantRepositoryMock;
        private Mock<IValidator<PaymentRequest>> _validatorMock;
        private Mock<IAcquiringBankClient> _acquiringBankMock;
        private Mock<IMapper> _autoMapperMock;
        private Mock<IDateTimeProvider> _dateTimeProviderMock;

        [SetUp]
        public void Setup() 
        {
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _merchantRepositoryMock = new Mock<IMerchantRepository>();
            _validatorMock = new Mock<IValidator<PaymentRequest>>();
            _acquiringBankMock = new Mock<IAcquiringBankClient>();
            _autoMapperMock = new Mock<IMapper>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        }

        [Test]
        public async Task ProcessPaymentRequest_NullPaymentRequest_Throws()
        {
            // Arrange
            PaymentRequest request = null;

            var sut = new PaymentService(
                _paymentRepositoryMock.Object,
                _merchantRepositoryMock.Object,
                _validatorMock.Object,
                _acquiringBankMock.Object,
                _autoMapperMock.Object,
                _dateTimeProviderMock.Object);

            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.ProcessPaymentRequest(request));
        }

        [Test]
        public async Task ProcessPaymentRequest_ValidationErrors_ReturnCorrectResponse()
        {
            // Arrange 
            PaymentRequest request = GetMockPaymentRequest();
            var mockMerchant = GetMockMerchant();
            var mockPayment = GetMappedPayment(request);
            var expectedPaymentGuid = new Guid("20b67363-5bb0-4d61-8dfa-1abc0bb2306f");

            DateTimeOffset statusTimeMock = new DateTimeOffset(2020, 01, 01, 0, 0, 0, new TimeSpan(0, 0, 0));

            _validatorMock.Setup(v => v.Validate(It.Is<PaymentRequest>(x => x.Equals(request))))
                .Returns(new List<string> { "Test validation failure" });

            _merchantRepositoryMock.Setup(mr => mr.GetMerchantById(It.Is<Guid>(x => x.Equals(mockMerchant.MerchantId))))
                .ReturnsAsync(mockMerchant);

            _autoMapperMock.Setup(am => am.Map<Payment>(It.Is<PaymentRequest>(x => x.Equals(request))))
                .Returns(mockPayment);

            _paymentRepositoryMock.Setup(pr => pr.Add(It.IsAny<Payment>())).ReturnsAsync((Payment p) =>
            {
                p.PaymentId = expectedPaymentGuid;
                return p;
            });

            _dateTimeProviderMock.Setup(dtp => dtp.UtcNow).Returns(statusTimeMock);

            var sut = new PaymentService(
                _paymentRepositoryMock.Object,
                _merchantRepositoryMock.Object,
                _validatorMock.Object,
                _acquiringBankMock.Object,
                _autoMapperMock.Object,
                _dateTimeProviderMock.Object);

            // Act 
            var response = await sut.ProcessPaymentRequest(request);

            // Assert
            Assert.AreEqual(expectedPaymentGuid, response.PaymentId);
            Assert.AreEqual(PaymentStatuses.InternalValidationError, response.PaymentStatus);
            Assert.Contains("Test validation failure", response.ValidationErrors);

            _paymentRepositoryMock.Verify(x => x.Add(It.IsAny<Payment>()), Times.Once());
        }

        [Test]
        public async Task ProcessPaymentRequest_BankClientErrors_ReturnCorrectResponse()
        {
            // Arrange 
            PaymentRequest request = GetMockPaymentRequest();
            var mockMerchant = GetMockMerchant();
            var mockPayment = GetMappedPayment(request);
            var expectedPaymentGuid = new Guid("20b67363-5bb0-4d61-8dfa-1abc0bb2306f");

            DateTimeOffset firstStatusTimeMock = new DateTimeOffset(2020, 01, 01, 0, 0, 0, new TimeSpan(0, 0, 0));
            DateTimeOffset secondStatusTimeMock = new DateTimeOffset(2020, 01, 01, 0, 0, 1, new TimeSpan(0, 0, 0));

            _validatorMock.Setup(v => v.Validate(It.Is<PaymentRequest>(x => x.Equals(request))))
                .Returns(new List<string> { });

            _merchantRepositoryMock.Setup(mr => mr.GetMerchantById(It.Is<Guid>(x => x.Equals(mockMerchant.MerchantId))))
                .ReturnsAsync(mockMerchant);

            _autoMapperMock.Setup(am => am.Map<Payment>(It.Is<PaymentRequest>(x => x.Equals(request))))
                .Returns(mockPayment);

            _paymentRepositoryMock.Setup(pr => pr.Add(It.IsAny<Payment>())).ReturnsAsync((Payment p) =>
            {
                p.PaymentId = expectedPaymentGuid;
                return p;
            });

            _paymentRepositoryMock.Setup(pr => pr.Update(It.IsAny<Payment>())).ReturnsAsync((Payment p) =>
            {
                return p;
            });

            _dateTimeProviderMock.SetupSequence(dtp => dtp.UtcNow)
                .Returns(firstStatusTimeMock)
                .Returns(secondStatusTimeMock);

            AcquiringBankPaymentDetails bankResponseMock = null;

            _acquiringBankMock.Setup(ab => ab.SubmitPaymentToBank(It.IsAny<PaymentRequest>())).ReturnsAsync(bankResponseMock);

            var sut = new PaymentService(
                _paymentRepositoryMock.Object,
                _merchantRepositoryMock.Object,
                _validatorMock.Object,
                _acquiringBankMock.Object,
                _autoMapperMock.Object,
                _dateTimeProviderMock.Object);

            // Act 
            var response = await sut.ProcessPaymentRequest(request);

            // Assert
            Assert.AreEqual(expectedPaymentGuid, response.PaymentId);
            Assert.AreEqual(PaymentStatuses.SubmissionError, response.PaymentStatus);
            Assert.IsNull(response.ValidationErrors);

            _paymentRepositoryMock.Verify(x => x.Add(It.IsAny<Payment>()), Times.Once());
            _paymentRepositoryMock.Verify(x => x.Update(It.IsAny<Payment>()), Times.Once());
        }

        [Test]
        public async Task ProcessPaymentRequest_ValidationErrors_ReturnCorrectResponse2()
        {
            // Arrange 
            PaymentRequest request = GetMockPaymentRequest();
            var mockMerchant = GetMockMerchant();
            var mockPayment = GetMappedPayment(request);
            var expectedPaymentGuid = new Guid("20b67363-5bb0-4d61-8dfa-1abc0bb2306f");

            DateTimeOffset firstStatusTimeMock = new DateTimeOffset(2020, 01, 01, 0, 0, 0, new TimeSpan(0, 0, 0));
            DateTimeOffset secondStatusTimeMock = new DateTimeOffset(2020, 01, 01, 0, 0, 1, new TimeSpan(0, 0, 0));

            _validatorMock.Setup(v => v.Validate(It.Is<PaymentRequest>(x => x.Equals(request))))
                .Returns(new List<string> { });

            _merchantRepositoryMock.Setup(mr => mr.GetMerchantById(It.Is<Guid>(x => x.Equals(mockMerchant.MerchantId))))
                .ReturnsAsync(mockMerchant);

            _autoMapperMock.Setup(am => am.Map<Payment>(It.Is<PaymentRequest>(x => x.Equals(request))))
                .Returns(mockPayment);

            _paymentRepositoryMock.Setup(pr => pr.Add(It.IsAny<Payment>())).ReturnsAsync((Payment p) =>
            {
                p.PaymentId = expectedPaymentGuid;
                return p;
            });

            _paymentRepositoryMock.Setup(pr => pr.Update(It.IsAny<Payment>())).ReturnsAsync((Payment p) =>
            {
                return p;
            });

            _dateTimeProviderMock.Setup(dtp => dtp.UtcNow).Returns(firstStatusTimeMock);

            AcquiringBankPaymentDetails bankResponseMock = new AcquiringBankPaymentDetails
            {
                BankId = new Guid("e5b9cb24-055c-4710-84a6-0b3b2496010b"),
                PaymentStatus = PaymentStatuses.Submitted,
                StatusDateTime = secondStatusTimeMock
            };

            _acquiringBankMock.Setup(ab => ab.SubmitPaymentToBank(It.IsAny<PaymentRequest>())).ReturnsAsync(bankResponseMock);

            var sut = new PaymentService(
                _paymentRepositoryMock.Object,
                _merchantRepositoryMock.Object,
                _validatorMock.Object,
                _acquiringBankMock.Object,
                _autoMapperMock.Object,
                _dateTimeProviderMock.Object);

            // Act 
            var response = await sut.ProcessPaymentRequest(request);

            // Assert
            Assert.AreEqual(expectedPaymentGuid, response.PaymentId);
            Assert.AreEqual(PaymentStatuses.Submitted, response.PaymentStatus);
            Assert.IsNull(response.ValidationErrors);

            _paymentRepositoryMock.Verify(x => x.Add(It.IsAny<Payment>()), Times.Once());
            _paymentRepositoryMock.Verify(x => x.Update(It.IsAny<Payment>()), Times.Once());
        }

        private PaymentRequest GetMockPaymentRequest()
        {
            return new PaymentRequest
            {
                Amount = 100,
                CardholderName = "Testy McTester",
                CardNumber = "1111 2222 3333 4444",
                CurrencyIsoAlpha3 = "GBP",
                Cvv = "123",
                MerchantId = new Guid("7b6bf96c-0231-41e3-9014-393ff87ed4e1")
            };
        }

        private Merchant GetMockMerchant()
        {
            return new Merchant
            {
                CreatedDate = new DateTimeOffset(2020, 01, 01, 0, 0, 0, new TimeSpan(0, 0, 0)),
                MerchantId = new Guid("7b6bf96c-0231-41e3-9014-393ff87ed4e1"),
                MerchantName = "Mr Testy"
            };
        }

        private Payment GetMappedPayment(PaymentRequest paymentRequest)
        {
            CardDetails cardDetails = new CardDetails
            {
                CardholderName = paymentRequest.CardholderName,
                CardNumber = paymentRequest.CardNumber,
                Cvv = paymentRequest.Cvv
            };

            return new Payment
            {
                Amount = paymentRequest.Amount,
                CardDetails = cardDetails,
                CurrencyIsoAlpha3 = paymentRequest.CurrencyIsoAlpha3
            };
        }
    }
}
