using System;
using System.Collections.Generic;

namespace PaymentGateway.Core.Models
{
    public class Payment
    {
        public Payment() 
        {
            PaymentStatuses = new List<PaymentStatus>();
        }

        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyIsoAlpha3 { get; set; }
        public CardDetails CardDetails { get; set; }
        public List<PaymentStatus> PaymentStatuses { get; set; }
        public Guid? BankId { get; set; }
    }
}
