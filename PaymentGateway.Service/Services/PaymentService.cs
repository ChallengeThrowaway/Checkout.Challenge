using PaymentGateway.Core.Models;
using PaymentGateway.Data.Entities;
using PaymentGateway.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

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
            var cardDetails = new Data.Entities.CardDetails
            {
                CardholderName = paymentRequest.CardholderName,
                CardNumber = paymentRequest.CardNumber,
                Ccv = paymentRequest.Ccv
            };

            var payment = new Data.Entities.Payment
            {
                Amount = paymentRequest.Amount,
                CardDetails = cardDetails,
                CurrencyIsoAlpha3 = "GBP"
            };

            payment.PaymentStatuses.Add(new Data.Entities.PaymentStatus
            {
                Status = Core.Enums.PaymentStatuses.PendingSubmission,
                StatusDateTime = DateTime.UtcNow
            });

            _paymentRepository.Add(payment);

            //TODO Send to acquiring bank using client
        }

        public Data.Entities.Payment GetPayment(Guid paymentGuid) 
        {
            return _paymentRepository.FindByPaymentId(paymentGuid);
        }
    }
}
