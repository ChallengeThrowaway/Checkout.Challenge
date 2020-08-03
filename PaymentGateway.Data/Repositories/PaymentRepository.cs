﻿using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentGatewayContext _context; 
        
        public PaymentRepository(PaymentGatewayContext context)
        {
            _context = context;            
        }

        public async Task<Payment> Add(Payment payment) 
        {
            _context.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> Update(Payment payment)
        {
            var oldPayment = await _context.Payments
                .Include(p => p.PaymentStatuses)
                .Where(p => p.PaymentId == payment.PaymentId)
                .FirstOrDefaultAsync();

            if (oldPayment != null)
            {
                _context.Entry(oldPayment).CurrentValues.SetValues(payment);
                await _context.SaveChangesAsync();
            }

            return payment;
        }

        public Task<Payment> FindByPaymentId(Guid paymentGuid)
        {
            return _context.Payments
                .Include(p => p.PaymentStatuses)
                .FirstOrDefaultAsync();
        }

    }
}
