using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Models;
using PaymentGateway.Core.Models;
using PaymentGateway.Service.Services;

namespace PaymentGateway.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
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
        public ActionResult<object> GetPayment(string id)
        {
            var payment = _paymentService.GetPayment(new System.Guid(id));
            return Ok(payment);
        }
    }
}
