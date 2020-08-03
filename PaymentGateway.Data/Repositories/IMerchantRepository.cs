using PaymentGateway.Data.Entities;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Repositories
{
    public interface IMerchantRepository
    {
        Task<Merchant> GetMerchantById(Guid merchantId);
    }
}
