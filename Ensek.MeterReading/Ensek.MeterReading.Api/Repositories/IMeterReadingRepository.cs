using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ensek.MeterReading.Api.Repositories
{
    public interface IMeterReadingRepository
    {
        public Task UploadMeterReading(List<Data.Model.MeterReading> meterReadings);
        public Task<Data.Model.MeterReading> GetMeterReading(Data.Model.MeterReading meterReading);
    }
}