using System.Collections.Generic;
using System.Threading.Tasks;
using Ensek.MeterReading.Api.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ensek.MeterReading.Api.Repositories
{
    public class MeterReadingRepository :IMeterReadingRepository
    {
        private readonly EnsekMeterReadingDbContext _dbContext;
        private ILogger<MeterReadingRepository> _logger;

        public MeterReadingRepository(EnsekMeterReadingDbContext dbContext,ILogger<MeterReadingRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task UploadMeterReading(List<Data.Model.MeterReading> meterReadings)
        {
            await _dbContext.MeterReading.AddRangeAsync(meterReadings);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Data.Model.MeterReading> GetMeterReading(Data.Model.MeterReading meterReading)
        {
            return await _dbContext.MeterReading.FirstOrDefaultAsync(m =>
                m.AccountId == meterReading.AccountId && m.MeterReadingAt >= meterReading.MeterReadingAt);
        }
    }
}