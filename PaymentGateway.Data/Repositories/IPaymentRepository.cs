using PaymentGateway.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Data.Repositories
{
    public interface IPaymentRepository
    {
        void Add(Payment payment);
        Payment FindByPaymentId(Guid paymentGuid);
    }
}
