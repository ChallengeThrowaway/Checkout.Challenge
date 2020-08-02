
namespace PaymentGateway.Api.Models
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string CurrencyIsoAlpha3 { get; set; }
        public string CardNumber { get; set; }
        public string Ccv { get; set; }
        public string CardholderName { get; set; }
    }
}
