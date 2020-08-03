using System;
using System.Collections.Generic;

namespace PaymentGateway.Api.Models
{
    public class PaymentResponse
    {
        public PaymentResponse()
        {
            ValidationErrors = new List<string>();
        }

        public Guid PaymentId { get; set; }
        public string PaymentStatus { get; set; }
        public List<string> ValidationErrors { get; set; }
    }
}
