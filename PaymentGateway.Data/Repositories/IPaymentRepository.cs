using PaymentGateway.Data.Entities;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Repositories
{
    public interface IPaymentRepository
    {
        void Add(Payment payment);
        Task<Payment> FindByPaymentId(Guid paymentGuid);
    }
}
