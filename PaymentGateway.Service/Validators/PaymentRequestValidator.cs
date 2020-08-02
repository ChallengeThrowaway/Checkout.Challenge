using PaymentGateway.Core.Models;
using PaymentGateway.Service.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace PaymentGateway.Service.Validators
{
    //TODO: Replace whole class with more robust validation, likely a third party solution
    public class PaymentRequestValidator : IValidator<PaymentRequest>
    {

        public List<string> Validate(PaymentRequest paymentRequest)
        {
            List<string> ValidationErrors = new List<string>();

            ValidationErrors.AddIfNotNull(ValidateAmount(paymentRequest));
            ValidationErrors.AddIfNotNull(ValidateCurrency(paymentRequest));
            ValidationErrors.AddIfNotNull(ValidateCardNumber(paymentRequest));
            ValidationErrors.AddIfNotNull(ValidateCcv(paymentRequest));
            ValidationErrors.AddIfNotNull(ValidateCardholderName(paymentRequest));

            return ValidationErrors;
        }

        private string ValidateAmount(PaymentRequest paymentRequest) 
        {
            return paymentRequest.Amount > 0 ? null : "Payment must be greater than 0";
        }

        private string ValidateCurrency(PaymentRequest paymentRequest)
        {
            return paymentRequest.CurrencyIsoAlpha3.Trim().Length == 3 ? null : "Currency must be in ISO 4217 Alpha 3 format";
        }

        // TODO: Add Luhn check to ensure valid card number
        public string ValidateCardNumber(PaymentRequest paymentRequest)
        {
            string strippedCardNumber = paymentRequest.CardNumber.Trim().Replace("-", ".");

            return strippedCardNumber.All(c => char.IsNumber(c)) && strippedCardNumber.Length == 16 ? null : "Card number must be 16 digits long, and contain only numbers";
        }

        private string ValidateCcv(PaymentRequest paymentRequest)
        {
            string strippedCcv = paymentRequest.Ccv.Trim();

            return strippedCcv.All(c => char.IsNumber(c)) && strippedCcv.Length == 3 ? null : "CCV must be 3 digits long, and only contain numbers";
        }

        private string ValidateCardholderName(PaymentRequest paymentRequest) 
        {
            return paymentRequest.CardholderName.Trim().All(c => char.IsLetter(c)) ? null : "Cardholder name must contain only letters";
        }
    }
}
