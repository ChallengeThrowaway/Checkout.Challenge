using AutoMapper;
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
        private readonly IMapper _autoMapper;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IValidator<PaymentRequest> paymentRequestValidator,
            IMapper autoMapper)
        {
            _paymentRepository = paymentRepository;
            _paymentRequestValidator = paymentRequestValidator;
            _autoMapper = autoMapper;
        }

        public void ProcessPaymentRequest(PaymentRequest paymentRequest)
        {
            var validationErrors = _paymentRequestValidator.Validate(paymentRequest);

            var payment = _autoMapper.Map<Payment>(paymentRequest);

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
