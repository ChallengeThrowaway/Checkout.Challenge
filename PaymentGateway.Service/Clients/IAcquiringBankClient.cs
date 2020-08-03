using System.Threading.Tasks;
using PaymentGateway.Core.Models;

namespace PaymentGateway.Service.Clients
{
    public interface IAcquiringBankClient
    {
        Task<AcquiringBankPaymentDetails> SubmitPaymentToBank(PaymentRequest paymentRequest);
    }
}
