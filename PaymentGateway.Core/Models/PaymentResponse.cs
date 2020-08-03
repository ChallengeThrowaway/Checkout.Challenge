using System;
using System.Collections.Generic;
using PaymentGateway.Core.Enums;

namespace PaymentGateway.Core.Models
{
    public class PaymentResponse
    {
        public PaymentResponse()
        {
            ValidationErrors = new List<string>();
        }

        public Guid PaymentId { get; set; }
        public PaymentStatuses PaymentStatus { get; set; }
        public List<string> ValidationErrors { get; set; }
    }
}
