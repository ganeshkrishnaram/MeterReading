using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using CsvHelper;
using Ensek.MeterReading.Api.Contracts;
using Ensek.MeterReading.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ensek.MeterReading.Api.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingRepository _meterReadingRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly List<MeterReadingDto> _meterReadingDtos;
        private ILogger<MeterReadingService> _logger;
        
        public MeterReadingService(IMeterReadingRepository meterReadingRepository,IAccountRepository accountRepository, List<MeterReadingDto> meterReadingDtos, ILogger<MeterReadingService> logger)
        {
            _accountRepository = accountRepository;
            _meterReadingRepository = meterReadingRepository;
            _logger = logger;
            _meterReadingDtos = meterReadingDtos;
        }
        
        public async Task<ReadingResult> ProcessMeterReadings(IFormFile meterReadingsCsv)
        {
            
            var validationResults = await ParseCsv(meterReadingsCsv);
            
            var meterReadings = MapToMeterReadings(_meterReadingDtos);
            
            var newMeterReadings = new List<Data.Model.MeterReading>();
            
            foreach (var meterReading in meterReadings)
            {
                var account = await _accountRepository.GetAccount(meterReading.AccountId);
                
                if (account == null)
                {
                    validationResults.Add(new ValidationResult(false)
                    {
                        ClientMessage = $"Account do not exist. Invalid Account Details [AccountId] : {meterReading.AccountId}",
                        
                    });
                    continue;
                }
                
                var reading = await _meterReadingRepository.GetMeterReading(meterReading);
                if (reading == null)
                {
                    validationResults.Add(new ValidationResult(true)
                    {
                        ClientMessage = $"Meter Reading successfully added for AccountId : {meterReading.AccountId}"
                    });
                    newMeterReadings.Add(meterReading);
                }
                else
                {
                    validationResults.Add(new ValidationResult(false)
                    {
                        ClientMessage = $"Meter Reading Already Exists for AccountId : {meterReading.AccountId}"
                    });
                }
            }

            if (newMeterReadings.Any())
            {
                await _meterReadingRepository.UploadMeterReading(newMeterReadings);
            }

            var readingResult = new ReadingResult
            {
                ValidationResults = validationResults,
                TotalSuccessfulReadings = validationResults.Count(x => x.IsValid == true),
                TotalFailedReadings = validationResults.Count(x => x.IsValid == false)
                
            };
            
            return readingResult;
        }

        private async Task<List<ValidationResult>> ParseCsv(IFormFile meterReadingsCsv)
        {
            using var reader = new StreamReader(meterReadingsCsv.OpenReadStream());
            using var csvReader = new CsvReader(reader, CultureInfo.GetCultureInfo("en-AU"));
            
            
            var validationResults = new List<ValidationResult>();
            
            while (await csvReader.ReadAsync())
            {
                try
                {
                    var record = csvReader.GetRecord<MeterReadingDto>();
                    switch (record.MeterReadValue)
                    {
                        case > 99999:
                            validationResults.Add(new ValidationResult(false)
                            {
                                ClientMessage = $"Reading Value for AccountId : {record.AccountId} exceeds the allowed number of digits NNNNN : {record.MeterReadValue}" 
                            });
                            break;
                        case < 0:
                            validationResults.Add(new ValidationResult(false)
                            {
                                ClientMessage = $"Reading Value for AccountId : {record.AccountId} has negative value : {record.MeterReadValue}" 
                            });
                            break;
                        case >= 0 and < 99999:
                            _meterReadingDtos.Add(record);
                            break;
                    }

                    
                }
                catch (Exception ex)
                {
                    validationResults.Add(new ValidationResult(false)
                    {
                        ClientMessage = $"Invalid CSV Record : {ex.Message}" 
                    });
                }
                
            }
            
            return validationResults;
            
        }    
        private static List<Data.Model.MeterReading> MapToMeterReadings(IEnumerable<MeterReadingDto> meterReadingDtos)
        {
            return meterReadingDtos.Select(reading => new Data.Model.MeterReading
            {
                AccountId = reading.AccountId,
                MeterReadingAt = reading.MeterReadingDateTime,
                MeterReadValue = reading.MeterReadValue
            }).ToList();
        }



    }
}