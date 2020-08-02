
using System;

namespace PaymentGateway.Core.Models
{
    public class CardDetails
    {
        public Guid CardDetailsId { get; set; }
        public string CardNumber { get; set; }
        public string Ccv { get; set; }
        public string CardholderName { get; set; }
    }
}
