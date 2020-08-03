using System;
using System.ComponentModel.DataAnnotations;
using PaymentGateway.Core.Enums;

namespace PaymentGateway.Data.Entities
{
    public class PaymentStatus
    {
        [Key]
        public Guid PaymentStatusId { get; set; }
        public PaymentStatuses StatusKey { get; set; }
        public DateTimeOffset StatusDateTime { get; set; }
    }
}
