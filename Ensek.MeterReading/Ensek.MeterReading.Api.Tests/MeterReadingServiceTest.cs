using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ensek.MeterReading.Api.Contracts;
using Ensek.MeterReading.Api.Data.Model;
using Ensek.MeterReading.Api.Repositories;
using Ensek.MeterReading.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Shouldly;

namespace Ensek.MeterReading.Api.Tests
{
    public class MeterReadingServiceTest
    {
        private Mock<IFormFile> _mockFile;
        private MeterReadingService _subject;
        private IMeterReadingRepository _fakeMeterReadingRepository;
        private IAccountRepository _fakeAccountRepository;
        private List<MeterReadingDto> _fakeMeterReadingDtos;
        private ILogger<MeterReadingService> _logger;
        
        [SetUp]
        public void Setup()
        {
            _fakeMeterReadingRepository = Substitute.For<IMeterReadingRepository>();
            _fakeAccountRepository = Substitute.For<IAccountRepository>();
            _fakeMeterReadingDtos = Substitute.For<List<MeterReadingDto>>();
            _logger = Substitute.For<ILogger<MeterReadingService>>();
            _subject = new MeterReadingService(_fakeMeterReadingRepository, _fakeAccountRepository, _fakeMeterReadingDtos, _logger);

        }

        [Test]
        public async Task GivenAnIncomingMeterReadingUploadRequest_WhenPreparingToProcessMeterReadingRecords_ThenAllTheValidMeterReadingRecordsIsAdded()
        {
            //Arrange
            
            PrepareMeterReadingUploadRequest();
            ArrangeAccounts();
            ArrangeMeterReadings();

            var expected = CreateMeterReadingResults(); 
            
            //Act
            var actual = await _subject.ProcessMeterReadings(_mockFile.Object);
            
            //Assert
            actual.ValidationResults.FirstOrDefault()?.IsValid.ShouldBeTrue();
            actual.ValidationResults.FirstOrDefault()?.ClientMessage.ShouldBeEquivalentTo(expected.FirstOrDefault()?.ClientMessage);
        }

        private void PrepareMeterReadingUploadRequest()
        {
            _mockFile = new Mock<IFormFile>();
            var csv = new StringBuilder();
            
            var newLine = "AccountId,MeterReadingDateTime,MeterReadValue\r\n";
            csv.Append(newLine);
            var content = "2344,20/12/2021 09:24,01002";
            csv.Append(content);
            var fileName = "MeterReading.csv";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(csv);
            writer.Flush();
            ms.Position = 0;
            
            _mockFile.Setup(_ => _.OpenReadStream()).Returns(ms);
            _mockFile.Setup(_ => _.FileName).Returns(fileName);
            _mockFile.Setup(_ => _.Length).Returns(ms.Length);
        }

        private void ArrangeAccounts()
        {
            var accountResponse = new Account
            {
                AccountId = 2344,
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName"
                
            };
            _fakeAccountRepository.GetAccount(2344).Returns(accountResponse);
        }
        
        private void ArrangeMeterReadings()
        {
            _fakeMeterReadingRepository.GetMeterReading(Arg.Any<Data.Model.MeterReading>())
                .ReturnsNull();
            
        }

        private IEnumerable<ValidationResult> CreateMeterReadingResults()
        {
            var validationResults = new List<ValidationResult>
            {
                new ValidationResult(true)
                {
                    ClientMessage = $"Meter Reading successfully added for AccountId : 2344"
                }
            };
            return validationResults;
        }
    }
}