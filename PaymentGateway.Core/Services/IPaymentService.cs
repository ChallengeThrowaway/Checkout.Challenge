using PaymentGateway.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Core.Services
{
    public interface IPaymentService
    {
        void CreatePayment(Payment payment);
    }
}
