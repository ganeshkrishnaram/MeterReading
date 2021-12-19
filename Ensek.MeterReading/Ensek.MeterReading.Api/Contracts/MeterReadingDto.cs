using System;

namespace Ensek.MeterReading.Api.Contracts
{
    public class MeterReadingDto
    {
        public int AccountId { get; set; }
        
        public DateTime MeterReadingDateTime { get; set; }
        
        public decimal MeterReadValue { get; set; }
    }
}