using Microsoft.EntityFrameworkCore;
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

        public Task<Payment> FindByPaymentId(Guid paymentId)
        {
            return _context.Payments
                .Include(p => p.PaymentStatuses)
                .Include(c => c.CardDetails)
                .Where(p => p.PaymentId == paymentId)
                .FirstOrDefaultAsync();
        }

        public Task<Payment> FindByPaymentAndMerchantId(Guid paymentId, Guid merchantId)
        {
            return _context.Payments
            .Include(p => p.PaymentStatuses)
            .Include(c => c.CardDetails)
            .Where(p => p.PaymentId == paymentId && p.Merchant.MerchantId == merchantId)
            .FirstOrDefaultAsync();
        }

    }
}
