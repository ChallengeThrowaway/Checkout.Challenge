using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Data.Entities
{
    public class Merchant
    {
        [Key]
        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
