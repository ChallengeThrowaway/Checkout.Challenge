using System.Threading.Tasks;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Data.Repositories
{
    public interface IApiKeyRepository
    {
        Task<ApiKey> FindByKey(string apiKey);
    }
}
