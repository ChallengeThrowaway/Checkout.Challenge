using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PaymentGateway.Data
{
    public class PaymentGatewayContextFactory : IDesignTimeDbContextFactory<PaymentGatewayContext>
    {
        public PaymentGatewayContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("GatewayConnectionString");
            var optionsBuilder = new DbContextOptionsBuilder<PaymentGatewayContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new PaymentGatewayContext(optionsBuilder.Options);
        }
    }
}
