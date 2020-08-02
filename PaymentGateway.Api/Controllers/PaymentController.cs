using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Models;
using PaymentGateway.Core.Models;
using PaymentGateway.Service.Services;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        //TODO: Add authentication, use authentication to link merchant to a payment so they can reconcile at a later date
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public ActionResult CreatePayment(Models.PaymentRequest payment)
        {
            var paymentRequest = new Core.Models.PaymentRequest
            {
                Amount = payment.Amount,
                CardholderName = payment.CardholderName,
                CardNumber = payment.CardNumber,
                Ccv = payment.Ccv,
                CurrencyIsoAlpha3 = payment.CurrencyIsoAlpha3
            };

            _paymentService.ProcessPaymentRequest(paymentRequest);
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
