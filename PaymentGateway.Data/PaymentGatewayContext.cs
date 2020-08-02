using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Data
{
    public class PaymentGatewayContext : DbContext
    {
        public PaymentGatewayContext(DbContextOptions<PaymentGatewayContext> options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<CardDetails> CardDetails { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
    }
}
