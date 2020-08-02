using PaymentGateway.Core.Enums;
using PaymentGateway.Core.Models;
using PaymentGateway.Data.Entities;
using PaymentGateway.Data.Repositories;
using PaymentGateway.Service.Validators;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IValidator<PaymentRequest> _paymentRequestValidator;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IValidator<PaymentRequest> paymentRequestValidator)
        {
            _paymentRepository = paymentRepository;
            _paymentRequestValidator = paymentRequestValidator;
        }

        public void ProcessPaymentRequest(PaymentRequest paymentRequest)
        {
            var validationErrors = _paymentRequestValidator.Validate(paymentRequest);

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
                Status = validationErrors.Count > 0 ? PaymentStatuses.ValidationError : PaymentStatuses.PendingSubmission,
                StatusDateTime = DateTime.UtcNow
            });

            _paymentRepository.Add(payment);

            //TODO Send to acquiring bank using client
        }

        public Task<Payment> GetPayment(Guid paymentGuid)
        {
            //TODO Mask card information
            return _paymentRepository.FindByPaymentId(paymentGuid);
        }
    }
}
