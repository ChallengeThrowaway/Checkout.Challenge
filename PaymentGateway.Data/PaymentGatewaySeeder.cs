using System;
using System.Linq;
using PaymentGateway.Data.Entities;

namespace PaymentGateway.Data
{
    public static class PaymentGatewaySeeder
    {
        public static void Seed(PaymentGatewayContext context)
        {
            // Check there are no existing merchants/api keys before migrating
            if (!context.Merchants.Any() && !context.ApiKeys.Any())
            {
                var seedMerchant1 = new Merchant
                {
                    MerchantId = new Guid("cbc193f2-76de-41d3-8ba4-025261a2069f"),
                    CreatedDate = new DateTimeOffset(2020, 08, 03, 0, 0, 0, 0, new TimeSpan(0, 0, 0)),
                    MerchantName = "Test Merchant 1"
                };

                var seedMerchant2 = new Merchant
                {
                    MerchantId = new Guid("2fbd9d9e-4555-4214-be35-a0d571c933d0"),
                    CreatedDate = new DateTimeOffset(2020, 08, 03, 0, 0, 0, 0, new TimeSpan(0, 0, 0)),
                    MerchantName = "Test Merchant 2"
                };

                context.Merchants.AddRange(new[] { seedMerchant1, seedMerchant2 });

                context.ApiKeys.Add(new ApiKey
                {
                    ApiKeyId = new Guid(),
                    Key = "9a3192fc-cb2d-487a-a377-8c6c8b60e007",
                    Owner = seedMerchant1,
                    ValidFrom = new DateTimeOffset(2020, 08, 03, 0, 0, 0, 0, new TimeSpan(0, 0, 0)),
                    ValidUntil = new DateTimeOffset(2022, 08, 03, 0, 0, 0, 0, new TimeSpan(0, 0, 0)),
                });

                context.ApiKeys.Add(new ApiKey
                {
                    ApiKeyId = new Guid(),
                    Key = "1215fbfb-7613-4810-bd1a-2e529c3484e9",
                    Owner = seedMerchant2,
                    ValidFrom = new DateTimeOffset(2020, 08, 03, 0, 0, 0, 0, new TimeSpan(0, 0, 0)),
                    ValidUntil = new DateTimeOffset(2022, 08, 03, 0, 0, 0, 0, new TimeSpan(0, 0, 0)),
                });

                context.SaveChanges();
            }

        }
    }
}
