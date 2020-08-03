using System;

namespace AcquiringBank.Mock.Models
{
    public class PaymentResponse
    {
        public string PaymentStatus { get; set; }
        public DateTimeOffset StatusDateTime { get; set; }
        public Guid BankId { get; set; }
    }
}
