using System;

namespace AcquiringBank.Mock.Models
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string CurrencyIsoAlpha3 { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public string CardholderName { get; set; }
        public Guid MerchantId { get; set; }
    }
}
