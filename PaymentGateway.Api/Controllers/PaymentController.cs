using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public ActionResult CreatePayment(PaymentRequest payment) 
        {
            return Ok();
        }

        [HttpGet]
        public ActionResult<object> GetPayment()
        {
            return Ok("Test");
        }
    }
}
