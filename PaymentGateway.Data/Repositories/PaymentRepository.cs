using PaymentGateway.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {

        private readonly PaymentGatewayContext _context; 
        
        public PaymentRepository(PaymentGatewayContext context)
        {
            _context = context;            
        }

        public void Add(Payment payment) 
        {
            _context.Add(payment);
            _context.SaveChanges();
        }

    }
}
