using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
