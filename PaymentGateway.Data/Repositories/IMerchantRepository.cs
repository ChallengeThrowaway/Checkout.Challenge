using System;
using System.Threading.Tasks;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Data.Repositories
{
    public interface IMerchantRepository
    {
        Task<Merchant> GetMerchantById(Guid merchantId);
    }
}
