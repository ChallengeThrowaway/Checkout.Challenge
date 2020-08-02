using PaymentGateway.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Data.Entities
{
    public class PaymentStatus
    {
        [Key]
        public Guid StatusId { get; set; }
        public PaymentStatuses Status { get; set; }
        public DateTimeOffset StatusDateTime { get; set; }
    }
}
