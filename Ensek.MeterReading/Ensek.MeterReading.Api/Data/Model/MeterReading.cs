using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CsvHelper.Configuration;

namespace Ensek.MeterReading.Api.Data.Model
{
    public class MeterReading
    {
        [Key]
        public int MeterReadingId { get; set; }

        public int AccountId { get; set; }
        
        public Account Account { get; set; }

        public DateTime MeterReadingAt { get; set; }
        
        public decimal MeterReadValue { get; set; }
    }
    
   
}