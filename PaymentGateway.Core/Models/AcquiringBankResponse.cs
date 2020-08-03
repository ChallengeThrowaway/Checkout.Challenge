using System;
using System.Text.Json.Serialization;

namespace PaymentGateway.Core.Models
{
    public class AcquiringBankResponse
    {
        [JsonPropertyName("paymentStatus")]
        public string PaymentStatus { get; set; }
        [JsonPropertyName("statusDateTime")]
        public DateTimeOffset StatusDateTime { get; set; }
        [JsonPropertyName("bankId")]
        public Guid BankId { get; set; }
    }
}
