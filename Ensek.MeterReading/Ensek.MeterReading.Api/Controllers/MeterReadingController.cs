using System;
using System.Net;
using System.Threading.Tasks;
using Ensek.MeterReading.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ensek.MeterReading.Api.Controllers
{
    public class MeterReadingController : ControllerBase
    {
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingController(IMeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }
        // GET
        [HttpPost("api/meter-reading-uploads")]
        public async Task<ActionResult> UploadMeterReading(IFormFile meterReadingCSV)
        {
            var result = await _meterReadingService.ProcessMeterReadings(meterReadingCSV);
            
            return Ok(result);
        }
        
    }
}