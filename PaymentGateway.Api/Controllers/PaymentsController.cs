using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Models;
using PaymentGateway.Service.Services;
using System.Security.Claims;
using System.Threading.Tasks;


namespace PaymentGateway.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        //TODO: Add authentication, use authentication to link merchant to a payment so they can reconcile at a later date
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

            await _paymentService.ProcessPaymentRequest(paymentRequest);
            return Ok();
        }

        [HttpGet]
        [Route("{Id}")]
        [Authorize]
        public async Task<ActionResult<PaymentDetails>> GetPayment(string id)
        {
            var payment = await _paymentService.GetPayment(new System.Guid(id));
            return Ok(payment);
        }
    }
}
