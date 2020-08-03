﻿using PaymentGateway.Core.Enums;
using PaymentGateway.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Clients
{
    public class AcquiringBankClient : IAcquiringBankClient
    {

        public AcquiringBankClient() { }

        public async Task<PaymentResponse> SubmitPaymentToBank(PaymentRequest paymentRequest)
        {
            if (paymentRequest.Cvv == "666")
            {
                return new PaymentResponse
                {
                    BankId = Guid.NewGuid(),
                    PaymentStatus = PaymentStatuses.SubmissionError,
                    StatusDateTime = DateTime.UtcNow
                };
            }

            else 
            {
                return new PaymentResponse
                {
                    BankId = Guid.NewGuid(),
                    PaymentStatus = PaymentStatuses.Submitted,
                    StatusDateTime = DateTime.UtcNow
                };
            }
        }
    }
}
