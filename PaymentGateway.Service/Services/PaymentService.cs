using PaymentGateway.Core.Enums;
using PaymentGateway.Core.Models;
using PaymentGateway.Data.Entities;
using PaymentGateway.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public void ProcessPaymentRequest(PaymentRequest paymentRequest)
        {
            var cardDetails = new CardDetails
            {
                CardholderName = paymentRequest.CardholderName,
                CardNumber = paymentRequest.CardNumber,
                Ccv = paymentRequest.Ccv
            };

            var payment = new Payment
            {
                Amount = paymentRequest.Amount,
                CardDetails = cardDetails,
                CurrencyIsoAlpha3 = paymentRequest.CurrencyIsoAlpha3
            };

            payment.PaymentStatuses.Add(new PaymentStatus
            {
                Status = PaymentStatuses.PendingSubmission,
                StatusDateTime = DateTime.UtcNow
            });


            _paymentRepository.Add(payment);

            //TODO Send to acquiring bank using client
        }

        public Task<Payment> GetPayment(Guid paymentGuid)
        {
            return _paymentRepository.FindByPaymentId(paymentGuid);
        }
    }
}
