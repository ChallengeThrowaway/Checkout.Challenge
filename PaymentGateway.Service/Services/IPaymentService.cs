using System;
using System.Threading.Tasks;
using PaymentGateway.Core.Models;

namespace PaymentGateway.Service.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPaymentRequest(PaymentRequest paymentRequest);
        Task<PaymentDetails> GetMerchantPaymentById(Guid paymentGuid, Guid merchantId);
    }
}
