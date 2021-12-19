using System;
using Ensek.MeterReading.Api.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Ensek.MeterReading.Api.Data.Context
{
    public class EnsekMeterReadingDbContext : DbContext
    {
        public DbSet<Account> Account { get; set; }
        public DbSet<Model.MeterReading> MeterReading { get; set; }
        
        public EnsekMeterReadingDbContext(DbContextOptions<EnsekMeterReadingDbContext> options)
            : base(options)
        {
            
        }
        
    }
}