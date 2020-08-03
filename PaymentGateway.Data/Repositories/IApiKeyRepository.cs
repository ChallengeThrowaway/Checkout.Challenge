using PaymentGateway.Data.Entities;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Repositories
{
    public interface IApiKeyRepository
    {
        Task<ApiKey> FindByKey(string apiKey);
    }
}
