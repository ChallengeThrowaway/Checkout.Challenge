using PaymentGateway.Core.Enums;
using System;

namespace PaymentGateway.Core.Models
{
    public class PaymentResponse
    {
        public PaymentStatuses PaymentStatus { get; set; }
        public DateTimeOffset StatusDateTime { get; set; }
        public Guid BankId { get; set; }
    }
}
