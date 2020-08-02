using PaymentGateway.Core.Models;
using PaymentGateway.Data.Entities;
using System;

namespace PaymentGateway.Service.Services
{
    public interface IPaymentService
    {
        void ProcessPaymentRequest(PaymentRequest paymentRequest);
        public Payment GetPayment(Guid paymentGuid);
    }
}
