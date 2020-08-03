using PaymentGateway.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Clients
{
    public interface IAcquiringBankClient
    {
        Task<PaymentResponse> SubmitPaymentToBank(PaymentRequest paymentRequest);
    }
}
