using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Data.Repositories
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly PaymentGatewayContext _context;

        public MerchantRepository(PaymentGatewayContext context)
        {
            _context = context;
        }

        public async Task<Merchant> GetMerchantById(Guid merchantId)
        {
            return await _context.Merchants
                .Where(m => m.MerchantId == merchantId)
                .FirstOrDefaultAsync();
        }
    }
}
