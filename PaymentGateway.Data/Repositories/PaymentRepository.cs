﻿using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;
using System;
using System.Linq;

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

        public Payment FindByPaymentId(Guid paymentGuid)
        {
            return _context.Payments
                .Include(p => p.PaymentStatuses)
                .FirstOrDefault();
        }

    }
}
