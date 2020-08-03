using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Models;
using PaymentGateway.Service.Services;
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
        public async Task<ActionResult> CreatePayment(PaymentRequest payment)
        {
            var paymentRequest = _autoMapper.Map<Core.Models.PaymentRequest>(payment);

            await _paymentService.ProcessPaymentRequest(paymentRequest);
            return Ok();
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<ActionResult<PaymentDetails>> GetPayment(string id)
        {
            var payment = await _paymentService.GetPayment(new System.Guid(id));
            return Ok(payment);
        }
    }
}
