using AcquiringBank.Mock.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AcquiringBank.Mock.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        [HttpPost]
        public ActionResult<PaymentResponse> CreatePayment(PaymentRequest payment)
        {
            var response =  new PaymentResponse
            {
                BankId = Guid.NewGuid(),
                PaymentStatus = "Success",
                StatusDateTime = DateTime.UtcNow
            };

            return Ok(response);
        }
    }
}
