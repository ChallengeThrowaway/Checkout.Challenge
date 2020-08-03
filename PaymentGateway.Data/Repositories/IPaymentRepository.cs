using PaymentGateway.Data.Entities;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> Add(Payment payment);
        Task<Payment> Update(Payment payment);
        Task<Payment> FindByPaymentId(Guid paymentGuid);
        Task<Payment> FindByPaymentAndMerchantId(Guid paymentGuid, Guid merchantId);
    }
}
