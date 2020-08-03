using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Data.Repositories
{
    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly PaymentGatewayContext _context;

        public ApiKeyRepository(PaymentGatewayContext context)
        {
            _context = context;
        }

        public async Task<ApiKey> FindByKey(string apiKey)
        {
            return await _context.ApiKeys
                .Include(o => o.Owner)
                .Where(a => a.Key == apiKey)
                .FirstOrDefaultAsync();
        }
    }
}
