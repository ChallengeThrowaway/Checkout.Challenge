using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PaymentGateway.Data.Entities
{
    public class Payment
    {
        [Key]
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyIsoAlpha3 { get; set; }
        public CardDetails CardDetails { get; set; }
        public virtual ICollection<PaymentStatus> PaymentStatuses { get; set; }
        public Guid? BankId { get; set; }
    }
}
