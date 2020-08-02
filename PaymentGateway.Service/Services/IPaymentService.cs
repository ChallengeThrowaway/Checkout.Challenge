using PaymentGateway.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Service.Services
{
    public interface IPaymentService
    {
        void ProcessPaymentRequest(PaymentRequest paymentRequest);
        public Data.Entities.Payment GetPayment(Guid paymentGuid);
    }
}
