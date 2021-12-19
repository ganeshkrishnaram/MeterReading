# Ensek Meter Reading Upload Feature Setup Instructions
EnsekMeterReading.db is a SQLite Database which stores the Test Account and Meter Reading Data

EnsekMeterReading.db is stored in the Data folder of the Ensek.MeterReading.Api.csproj

EntityFramework Core is leveraged to handle the SQLite Database operations

The initial seed of Test_Account.csv as well as execution of migration scripts is automatically handled during the Application Startup through WebHostExtensions.Please refer to host.Initialize.Run() available in program.cs.

Migration Scripts are stored in the Migration

MeterReadingService handles the validation,Processing and uploading of Meter_Reading.csv by leveraging the AccountRepository and MeterReadingRepository. 

AccountRepository and MeterReadingRepository handles the database CRUD operations of Account and Meter Reading table.

Dependency Injection pattern is leveraged to handle all the MeterReading related business services and repositories

TDD practice is adapted to test the Meter Reading Upload feature.

NUnit,NSubstitute,MOQ and Shouldly being used to implement the unit testing for Meter Reading Upload feature.

**ASSUMPTIONS**

Reading Value lesser than zero or greater than 99999(NNNNN) is considered as Invalid. Reading value of zero is assumed as valid meter reading value for this purpose of exercise

**Note:**

Please refer to the totalSuccessfulReadings and totalFailedReadings property available in the response returned for this meter-reading-uploads API.

![image](https://user-images.githubusercontent.com/36460347/146676608-7be306e1-3dba-4c82-b3a6-24c330f4b284.png)
