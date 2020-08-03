﻿using System;

namespace PaymentGateway.Core.Models
{
    public class AcquiringBankResponse
    {
        public string PaymentStatus { get; set; }
        public DateTimeOffset StatusDateTime { get; set; }
        public Guid BankId { get; set; }
    }
}
