using PaymentGateway.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

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
