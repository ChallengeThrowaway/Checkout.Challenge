using AutoMapper;
using PaymentGateway.Core.Enums;
using PaymentGateway.Core.Models;
using PaymentGateway.Data.Entities;
using PaymentGateway.Data.Repositories;
using PaymentGateway.Service.Clients;
using PaymentGateway.Service.Extensions;
using PaymentGateway.Service.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IValidator<PaymentRequest> _paymentRequestValidator;
        private readonly IAcquiringBankClient _acquiringBankClient;
        private readonly IMapper _autoMapper;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IMerchantRepository merchantRepository,
            IValidator<PaymentRequest> paymentRequestValidator,
            IAcquiringBankClient acquiringBankClient,
            IMapper autoMapper)
        {
            _paymentRepository = paymentRepository;
            _merchantRepository = merchantRepository;
            _paymentRequestValidator = paymentRequestValidator;
            _acquiringBankClient = acquiringBankClient;
            _autoMapper = autoMapper;
        }

        public async Task<PaymentResponse> ProcessPaymentRequest(PaymentRequest paymentRequest)
        {
            var validationErrors = _paymentRequestValidator.Validate(paymentRequest);

            var merchant = _merchantRepository.GetMerchantById(paymentRequest.MerchantId);

            var payment = _autoMapper.Map<Payment>(paymentRequest);

            payment.Merchant = await merchant;

            payment.PaymentStatuses.Add(new PaymentStatus
            {
                StatusKey = validationErrors.Any() ? PaymentStatuses.InternalValidationError : PaymentStatuses.PendingSubmission,
                StatusDateTime = DateTime.UtcNow
            });

            payment = await _paymentRepository.Add(payment);

            if (validationErrors.Any()) 
            {
                return GenerateNewPaymentResponse(payment, validationErrors);
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

                return GenerateNewPaymentResponse(payment);
            }

            payment.BankId = bankResponse.BankId;
            payment.PaymentStatuses.Add(new PaymentStatus
            {
                StatusKey = bankResponse.PaymentStatus,
                StatusDateTime = bankResponse.StatusDateTime
            });

            payment = await _paymentRepository.Update(payment);

            return GenerateNewPaymentResponse(payment);
        }

        public async Task<PaymentDetails> GetMerchantPaymentById(Guid paymentId, Guid merchantId)
        {
            //TODO Mask card information
            var payment =  await _paymentRepository.FindByPaymentAndMerchantId(paymentId, merchantId);

            return new PaymentDetails
            {
                Amount = payment.Amount,
                CardholderName = payment.CardDetails.CardholderName,
                CurrencyIsoAlpha3 = payment.CurrencyIsoAlpha3,
                LatestPaymentStatus = payment.PaymentStatuses.OrderByDescending(d => d.StatusDateTime).FirstOrDefault().StatusKey.ToString(),
                MaskedCardNumber = payment.CardDetails.CardNumber.FormatMaskedCardDetails()
            };
        }

        private PaymentResponse GenerateNewPaymentResponse(Payment payment, List<string> validationErrors = null)
        {
            return new PaymentResponse
            {
                PaymentId = payment.PaymentId,
                PaymentStatus = payment.PaymentStatuses.OrderByDescending(d => d.StatusDateTime).FirstOrDefault().StatusKey,
                ValidationErrors = validationErrors
            };
        }
    }
}
