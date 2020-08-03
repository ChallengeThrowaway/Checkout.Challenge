using PaymentGateway.Core.Enums;
using System;

namespace PaymentGateway.Core.Models
{
    public class AcquiringBankPaymentDetails
    {
        public PaymentStatuses PaymentStatus { get; set; }
        public DateTimeOffset StatusDateTime { get; set; }
        public Guid BankId { get; set; }
    }
}
