using NUnit.Framework;
using PaymentGateway.Core.Models;
using PaymentGateway.Service.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Service.Tests.Validators
{
    [TestFixture]
    public class PaymentRequestValidatorTests
    {
        [TestCase("veryInvalidCvv")]
        [TestCase("1000")]
        [TestCase("-100")]
        [TestCase("£$&£$")]
        public void Validate_InvalidCvv_ReturnsValidationError(string testCcv) 
        {
            // Arrange 
            PaymentRequest request = new PaymentRequest
            {
                Amount = 100,
                CardholderName = "Valid Name",
                CardNumber = "1111 2222 3333 4444",
                CurrencyIsoAlpha3 = "GBP",
                Cvv = testCcv
            };

            var sut = new PaymentRequestValidator();

            // Act
            var result = sut.Validate(request);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.Contains("CVV must be 3 digits long, and only contain numbers", result);
        }

        [TestCase(-100000)]
        [TestCase(-1)]
        [TestCase(0)]
        public void Validate_InvalidAmount_ReturnsValidationError(decimal testAmount) 
        {
            // Arrange 
            PaymentRequest request = new PaymentRequest
            {
                Amount = testAmount,
                CardholderName = "Valid Name",
                CardNumber = "1111 2222 3333 4444",
                CurrencyIsoAlpha3 = "GBP",
                Cvv = "123"
            };

            var sut = new PaymentRequestValidator();

            // Act
            var result = sut.Validate(request);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.Contains("Payment must be greater than 0", result);
        }

        [TestCase("veryInvalidCurrency")]
        [TestCase("1000")]
        [TestCase("GBPR")]
        [TestCase("GB")]
        [TestCase("£$&£$")]
        public void Validate_InvalidCurrency_ReturnsValidationError(string testCurrency)
        {
            // Arrange 
            PaymentRequest request = new PaymentRequest
            {
                Amount = 100,
                CardholderName = "Valid Name",
                CardNumber = "1111 2222 3333 4444",
                CurrencyIsoAlpha3 = testCurrency,
                Cvv = "123"
            };

            var sut = new PaymentRequestValidator();

            // Act
            var result = sut.Validate(request);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.Contains("Currency must be in ISO 4217 Alpha 3 format", result);
        }

        [TestCase("veryInvalidCardNumber")]
        [TestCase("1111.2222.3333.4444")]
        [TestCase("1111-2222-33333-4444")]
        [TestCase("111122233334444")]
        [TestCase("£$&£$")]
        public void Validate_InvalidCardNumber_ReturnsValidationError(string testCardNumber)
        {
            // Arrange 
            PaymentRequest request = new PaymentRequest
            {
                Amount = 100,
                CardholderName = "Valid Name",
                CardNumber = testCardNumber,
                CurrencyIsoAlpha3 = "GBP",
                Cvv = "123"
            };

            var sut = new PaymentRequestValidator();

            // Act
            var result = sut.Validate(request);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.Contains("Card number must be 16 digits long, and contain only numbers", result);
        }

        [TestCase("testy mctester1")]
        [TestCase("test.mctester")]
        [TestCase("testy-mctester")]
        [TestCase("")]
        [TestCase("te")]
        [TestCase("£$&£$")]
        public void Validate_InvalidCardholderName_ReturnsValidationError(string testCardholder)
        {
            // Arrange 
            PaymentRequest request = new PaymentRequest
            {
                Amount = 100,
                CardholderName =  testCardholder,
                CardNumber = "1111 2222 3333 4444",
                CurrencyIsoAlpha3 = "GBP",
                Cvv = "123"
            };

            var sut = new PaymentRequestValidator();

            // Act
            var result = sut.Validate(request);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.Contains("Cardholder name must contain only letters", result);
        }

        public void Validate_ValidPaymentRequest_ReturnsNoValidationErrors() 
        {
            // Arrange 
            PaymentRequest request = new PaymentRequest
            {
                Amount = 100,
                CardholderName = "Testy McTester",
                CardNumber = "1111 2222 3333 4444",
                CurrencyIsoAlpha3 = "GBP",
                Cvv = "123"
            };

            var sut = new PaymentRequestValidator();

            // Act
            var result = sut.Validate(request);

            // Assert
            Assert.IsEmpty(result);
        }
    }
}
