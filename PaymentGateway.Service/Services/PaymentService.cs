using AutoMapper;
using PaymentGateway.Core.Enums;
using PaymentGateway.Core.Models;
using PaymentGateway.Data.Entities;
using PaymentGateway.Data.Repositories;
using PaymentGateway.Service.Clients;
using PaymentGateway.Service.Validators;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IValidator<PaymentRequest> _paymentRequestValidator;
        private readonly IAcquiringBankClient _acquiringBankClient;
        private readonly IMapper _autoMapper;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IValidator<PaymentRequest> paymentRequestValidator,
            IAcquiringBankClient acquiringBankClient,
            IMapper autoMapper)
        {
            _paymentRepository = paymentRepository;
            _paymentRequestValidator = paymentRequestValidator;
            _acquiringBankClient = acquiringBankClient;
            _autoMapper = autoMapper;
        }

        public async Task ProcessPaymentRequest(PaymentRequest paymentRequest)
        {
            var validationErrors = _paymentRequestValidator.Validate(paymentRequest);

            var payment = _autoMapper.Map<Payment>(paymentRequest);

            payment.PaymentStatuses.Add(new PaymentStatus
            {
                StatusKey = validationErrors.Count > 0 ? PaymentStatuses.InternalValidationError : PaymentStatuses.PendingSubmission,
                StatusDateTime = DateTime.UtcNow
            });

            payment = await _paymentRepository.Add(payment);

            //TODO Need to return some sort of failure message here
            if (validationErrors.Count > 0) 
            {
                return;
            }

            //TODO Send to acquiring bank using client
            var bankResponse = await _acquiringBankClient.SubmitPaymentToBank(paymentRequest);

            if (bankResponse == null)
            {
                payment.PaymentStatuses.Add(new PaymentStatus {
                    StatusKey = PaymentStatuses.SubmissionError,
                    StatusDateTime = DateTime.UtcNow
                });

                await _paymentRepository.Update(payment);

                return;
            }

            payment.BankId = bankResponse.BankId;
            payment.PaymentStatuses.Add(new PaymentStatus
            {
                StatusKey = bankResponse.PaymentStatus,
                StatusDateTime = bankResponse.StatusDateTime
            });

            await _paymentRepository.Update(payment);
        }

        //TODO: Only return payment if merchant who submitted payment is the one requesting it
        public Task<Payment> GetPayment(Guid paymentGuid)
        {
            //TODO Mask card information
            return _paymentRepository.FindByPaymentId(paymentGuid);
        }
    }
}
