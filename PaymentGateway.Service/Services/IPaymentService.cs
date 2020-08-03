using PaymentGateway.Core.Models;
using PaymentGateway.Data.Entities;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPaymentRequest(PaymentRequest paymentRequest);
        Task<Payment> GetMerchantPaymentById(Guid paymentGuid, Guid merchantId);
    }
}
