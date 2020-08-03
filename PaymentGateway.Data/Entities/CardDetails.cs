using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Data.Entities
{
    public class CardDetails
    {
        [Key]
        public Guid CardDetailsId { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public string CardholderName { get; set; }
    }
}
