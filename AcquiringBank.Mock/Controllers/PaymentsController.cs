using System;
using AcquiringBank.Mock.Models;
using Microsoft.AspNetCore.Mvc;


namespace AcquiringBank.Mock.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        [HttpPost]
        public ActionResult<PaymentResponse> CreatePayment(PaymentRequest payment)
        {

            switch (payment.Cvv)
            {
                case "600":
                    return GenerateResponse("submitted");
                case "666":
                    return GenerateResponse("ValidationError");
                case "700":
                    return GenerateResponse("SubmissionError");
                default:
                    return GenerateResponse("Submitted");
            }
        }

        private PaymentResponse GenerateResponse(string status)
        {
            return new PaymentResponse
            {
                BankId = Guid.NewGuid(),
                PaymentStatus = status,
                StatusDateTime = DateTime.UtcNow
            };
        }
    }
}
