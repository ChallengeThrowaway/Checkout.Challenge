﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Models;
using PaymentGateway.Service.Services;

namespace PaymentGateway.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _autoMapper;

        public PaymentsController(
            IPaymentService paymentService,
            IMapper autoMapper)
        {
            _paymentService = paymentService;
            _autoMapper = autoMapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreatePayment(PaymentRequest payment)
        {
            var merchantId = User.Identity.Name;

            var paymentRequest = _autoMapper.Map<Core.Models.PaymentRequest>(payment);
            paymentRequest.MerchantId = new Guid(merchantId);

            var response = await _paymentService.ProcessPaymentRequest(paymentRequest);

            //TODO Find a way to improve this
            var paymentResponse = new PaymentResponse
            {
                PaymentId = response.PaymentId,
                ValidationErrors = response.ValidationErrors,
                PaymentStatus = response.PaymentStatus.ToString()
            };

            return Ok(paymentResponse);
        }

        [HttpGet]
        [Route("{paymentId}")]
        [Authorize]
        public async Task<ActionResult<PaymentDetails>> GetPayment(string paymentId)
        {
            var merchantId = User.Identity.Name;

            var payment = await _paymentService.GetMerchantPaymentById(new Guid(paymentId), new Guid(merchantId));

            //TODO Dont give everything in response, need new response model

            return Ok(payment);
        }
    }
}
