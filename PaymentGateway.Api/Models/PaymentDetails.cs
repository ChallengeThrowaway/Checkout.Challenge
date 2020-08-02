using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Models
{
    public class PaymentResponse
    {
        public decimal Amount { get; set; }
        public string CurrencyIsoAlpha3 { get; set; }
        public string MaskedCardNumber { get; set; }
        public string CardholderName { get; set; }
        public string LatestPaymentStatus { get; set; }
    }
}
