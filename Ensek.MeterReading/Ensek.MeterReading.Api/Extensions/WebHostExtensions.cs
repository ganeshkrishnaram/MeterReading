using System;
using Ensek.MeterReading.Api.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ensek.MeterReading.Api.Extensions
{
    public static class WebHostExtensions
    {
        public static IHost Initialize(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<EnsekMeterReadingDbContext>();
                var env = services.GetRequiredService<IWebHostEnvironment>();
                context.Database.Migrate();
                var ensekMeterReadingDbInitializer = new EnsekMeterReadingDbInitializer(context,env);
                ensekMeterReadingDbInitializer.SeedAccountDetails();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred initializing the DB");
            }

            return host;
        }
    }
}