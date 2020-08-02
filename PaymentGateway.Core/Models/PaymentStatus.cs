using System;
using PaymentGateway.Core.Enums;

namespace PaymentGateway.Core.Models
{
    public class PaymentStatus
    {
        public Guid StatusId { get; set; }
        public PaymentStatuses Status { get; set; }
        public DateTimeOffset StatusDateTime { get; set; }
    }
}
