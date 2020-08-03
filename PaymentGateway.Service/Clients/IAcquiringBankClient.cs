using PaymentGateway.Core.Models;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Clients
{
    public interface IAcquiringBankClient
    {
        Task<AcquiringBankPaymentDetails> SubmitPaymentToBank(PaymentRequest paymentRequest);
    }
}
