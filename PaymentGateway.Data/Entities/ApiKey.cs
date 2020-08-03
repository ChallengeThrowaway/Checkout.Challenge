using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Data.Entities
{
    public class ApiKey
    {
        [Key]
        public int ApiKeyId { get; set; }
        public Merchant Owner { get; set; }
        public string Key { get; set; }
        public DateTimeOffset ValidFrom { get; set; }
        public DateTimeOffset ValidUntil { get; set; }
    }
}
