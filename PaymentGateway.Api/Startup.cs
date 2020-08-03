using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Api.Authentication;
using PaymentGateway.Api.Middleware;
using PaymentGateway.Core.Configuration;
using PaymentGateway.Core.Helpers;
using PaymentGateway.Core.Models;
using PaymentGateway.Data;
using PaymentGateway.Data.Repositories;
using PaymentGateway.Service.Clients;
using PaymentGateway.Service.Services;
using PaymentGateway.Service.Validators;

namespace PaymentGateway.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            }).AddApiKeySupport(options => { });

            services.AddDbContext<PaymentGatewayContext>(options => options.UseSqlServer(Configuration.GetConnectionString("GatewayConnectionString")));
            services.Configure<AcquiringBankSettings>(options => Configuration.GetSection("AcquiringBankSettings").Bind(options));

            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IApiKeyRepository, ApiKeyRepository>();
            services.AddTransient<IMerchantRepository, MerchantRepository>();
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            services.AddSingleton<IValidator<PaymentRequest>, PaymentRequestValidator>();
            services.AddHttpClient<IAcquiringBankClient, AcquiringBankClient>();

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                MigrateAndSeedDatabase(app);
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("health");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway API v1");
            });
        }

        private static void MigrateAndSeedDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<PaymentGatewayContext>())
                {
                    context.Database.Migrate();
                    PaymentGatewaySeeder.Seed(context);
                }
            }
        }
    }
}
