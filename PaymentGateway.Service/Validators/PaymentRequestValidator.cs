﻿using PaymentGateway.Core.Models;
using PaymentGateway.Service.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace PaymentGateway.Service.Validators
{
    //TODO: Replace whole class with more robust validation, likely a third party solution
    //TODO: Replace with two seperate validators, one for the payment and one for the card details, really shouldn't be in one
    public class PaymentRequestValidator : IValidator<PaymentRequest>
    {

        public List<string> Validate(PaymentRequest paymentRequest)
        {
            List<string> ValidationErrors = new List<string>();

            ValidationErrors.AddIfNotNull(ValidateAmount(paymentRequest));
            ValidationErrors.AddIfNotNull(ValidateCurrency(paymentRequest));
            ValidationErrors.AddIfNotNull(ValidateCardNumber(paymentRequest));
            ValidationErrors.AddIfNotNull(ValidateCvv(paymentRequest));
            ValidationErrors.AddIfNotNull(ValidateCardholderName(paymentRequest));

            return ValidationErrors;
        }

        private string ValidateAmount(PaymentRequest paymentRequest) 
        {
            return paymentRequest.Amount > 0 ? null : "Payment must be greater than 0";
        }

        private string ValidateCurrency(PaymentRequest paymentRequest)
        {
            return paymentRequest.CurrencyIsoAlpha3.Replace(" ", "").Length == 3 ? null : "Currency must be in ISO 4217 Alpha 3 format";
        }

        // TODO: Add Luhn check to ensure valid card number
        public string ValidateCardNumber(PaymentRequest paymentRequest)
        {
            string strippedCardNumber = paymentRequest.CardNumber.Replace(" ", "").Replace("-", ".");

            return strippedCardNumber.All(c => char.IsNumber(c)) && strippedCardNumber.Length == 16 ? null : "Card number must be 16 digits long, and contain only numbers";
        }

        private string ValidateCvv(PaymentRequest paymentRequest)
        {
            string strippedCvv = paymentRequest.Cvv.Replace(" ","");

            return strippedCvv.All(c => char.IsNumber(c)) && strippedCvv.Length == 3 ? null : "CVV must be 3 digits long, and only contain numbers";
        }

        private string ValidateCardholderName(PaymentRequest paymentRequest) 
        {
            return paymentRequest.CardholderName.Replace(" ", "").All(c => char.IsLetter(c)) ? null : "Cardholder name must contain only letters";
        }
    }
}
