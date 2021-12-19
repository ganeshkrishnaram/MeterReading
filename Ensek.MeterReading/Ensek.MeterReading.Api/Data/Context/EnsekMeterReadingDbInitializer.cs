using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Ensek.MeterReading.Api.Data.Model;
using Microsoft.AspNetCore.Hosting;

namespace Ensek.MeterReading.Api.Data.Context
{
    public class EnsekMeterReadingDbInitializer
    {
 
        private readonly EnsekMeterReadingDbContext _db;
        private readonly IWebHostEnvironment _env;
        
        public EnsekMeterReadingDbInitializer(EnsekMeterReadingDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
            
        }
        public void SeedAccountDetails()
        {
            if (_db.Account.Any()) return;
            
            const string resourceName = "Ensek.MeterReading.Api.Data.Seeders.Test_Accounts.csv";
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (stream == null) throw new ConfigurationException("Test_Accounts.csv file not configured properly");
            using var reader = new StreamReader(stream, Encoding.UTF8);
            var csvReader = new CsvReader(reader,CultureInfo.InvariantCulture);
            var accounts = csvReader.GetRecords<Account>().ToList();
            _db.Account.AddRange(accounts);
            _db.SaveChanges();
        }
    }
}