using PaymentGateway.Core.Models;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPaymentRequest(PaymentRequest paymentRequest);
        Task<PaymentDetails> GetMerchantPaymentById(Guid paymentGuid, Guid merchantId);
    }
}
