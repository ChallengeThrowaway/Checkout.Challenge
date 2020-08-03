using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PaymentGateway.Core.Configuration;
using PaymentGateway.Core.Enums;
using PaymentGateway.Core.Models;
using PaymentGateway.Service.Clients;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Tests.Clients
{
    [TestFixture]
    public class AcquiringBankClientTests
    {
        private readonly Mock<IOptions<AcquiringBankSettings>> _acquiringBankSettingsMock;
        private readonly Mock<ILogger<AcquiringBankClient>> _loggerMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

        public AcquiringBankClientTests()
        {
            _acquiringBankSettingsMock = new Mock<IOptions<AcquiringBankSettings>>();
            _loggerMock = new Mock<ILogger<AcquiringBankClient>>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        }

        [SetUp]
        public void Setup()
        {
            var mockOptions = new AcquiringBankSettings
            {
                BaseUrl = "http://testUrl"
            };

            _acquiringBankSettingsMock.Setup(abs => abs.Value).Returns(mockOptions);
        }

        [Test]
        public async Task SubmitPaymentToBank_NullPaymentRequest_Throws()
        {
            // Arrange
            PaymentRequest paymentRequest = null;

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var client = new HttpClient(_httpMessageHandlerMock.Object);

            var sut = new AcquiringBankClient(_acquiringBankSettingsMock.Object, client, _loggerMock.Object);

            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => sut.SubmitPaymentToBank(paymentRequest));
        }

        [Test]
        public async Task SubmitPaymentToBank_ValidPaymentRequest_ReturnsCorrectDetails()
        {
            // Arrange
            var expectedBankId = new Guid("7b6bf96c-0231-41e3-9014-393ff87ed4e1");

            PaymentRequest paymentRequest = new PaymentRequest
            {
                Amount = 100,
                CardholderName = "Testy McTester",
                CardNumber = "1111 2222 3333 4444",
                CurrencyIsoAlpha3 = "GBP",
                Cvv = "123",
                MerchantId = new Guid("aa9f4ef9-35ab-4a10-8cf5-f5535582fd01")
            };

            var submissionDateTimeMock = new DateTimeOffset(2020, 01, 01, 0, 0, 0, new TimeSpan(0, 0, 0));

            AcquiringBankResponse responseMock = new AcquiringBankResponse
            {
                BankId = expectedBankId,
                PaymentStatus = "Submitted",
                StatusDateTime = submissionDateTimeMock
            };

            var data = JsonSerializer.Serialize(responseMock);
            var contentMock = new StringContent(data.ToString(), Encoding.UTF8, "application/json");

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = contentMock
                });

            var client = new HttpClient(_httpMessageHandlerMock.Object);

            var sut = new AcquiringBankClient(_acquiringBankSettingsMock.Object, client, _loggerMock.Object);

            // Act 
            var result = await sut.SubmitPaymentToBank(paymentRequest);

            // Assert
            Assert.AreEqual(expectedBankId, result.BankId);
            Assert.AreEqual(PaymentStatuses.Submitted, result.PaymentStatus);
            Assert.AreEqual(submissionDateTimeMock, result.StatusDateTime);
        }

        [Test]
        public async Task SubmitPaymentToBank_ValidationErrorPaymentRequest_ReturnsCorrectDetails()
        {
            // Arrange
            PaymentRequest paymentRequest = new PaymentRequest
            {
                Amount = 100,
                CardholderName = "Testy McTester",
                CardNumber = "1111 2222 3333 4444",
                CurrencyIsoAlpha3 = "GBP",
                Cvv = "666",
                MerchantId = new Guid("aa9f4ef9-35ab-4a10-8cf5-f5535582fd01")
            };

            var submissionDateTimeMock = new DateTimeOffset(2020, 01, 01, 0, 0, 0, new TimeSpan(0, 0, 0));

            AcquiringBankResponse responseMock = new AcquiringBankResponse
            {
                PaymentStatus = "ValidationError",
                StatusDateTime = submissionDateTimeMock
            };

            var data = JsonSerializer.Serialize(responseMock);
            var contentMock = new StringContent(data.ToString(), Encoding.UTF8, "application/json");

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = contentMock
                });

            var client = new HttpClient(_httpMessageHandlerMock.Object);

            var sut = new AcquiringBankClient(_acquiringBankSettingsMock.Object, client, _loggerMock.Object);

            // Act 
            var result = await sut.SubmitPaymentToBank(paymentRequest);

            // Assert
            Assert.AreEqual(PaymentStatuses.BankValidationError, result.PaymentStatus);
            Assert.AreEqual(submissionDateTimeMock, result.StatusDateTime);
        }

        [Test]
        public async Task SubmitPaymentToBank_SubmissionErrorPaymentRequest_ReturnsCorrectDetails()
        {
            // Arrange
            PaymentRequest paymentRequest = new PaymentRequest
            {
                Amount = 100,
                CardholderName = "Testy McTester",
                CardNumber = "1111 2222 3333 4444",
                CurrencyIsoAlpha3 = "GBP",
                Cvv = "666",
                MerchantId = new Guid("aa9f4ef9-35ab-4a10-8cf5-f5535582fd01")
            };

            var submissionDateTimeMock = new DateTimeOffset(2020, 01, 01, 0, 0, 0, new TimeSpan(0, 0, 0));

            AcquiringBankResponse responseMock = new AcquiringBankResponse
            {
                PaymentStatus = "SubmissionError",
                StatusDateTime = submissionDateTimeMock
            };

            var data = JsonSerializer.Serialize(responseMock);
            var contentMock = new StringContent(data.ToString(), Encoding.UTF8, "application/json");

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = contentMock
                });

            var client = new HttpClient(_httpMessageHandlerMock.Object);

            var sut = new AcquiringBankClient(_acquiringBankSettingsMock.Object, client, _loggerMock.Object);

            // Act 
            var result = await sut.SubmitPaymentToBank(paymentRequest);

            // Assert
            Assert.AreEqual(PaymentStatuses.SubmissionError, result.PaymentStatus);
            Assert.AreEqual(submissionDateTimeMock, result.StatusDateTime);
        }
    }
}
