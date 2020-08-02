using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public ActionResult CreatePayment() 
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
