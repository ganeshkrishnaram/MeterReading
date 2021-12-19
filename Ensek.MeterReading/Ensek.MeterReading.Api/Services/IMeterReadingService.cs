using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ensek.MeterReading.Api.Services
{
    public interface IMeterReadingService
    {
        public Task<ReadingResult> ProcessMeterReadings(IFormFile meterReadings);
    }
}