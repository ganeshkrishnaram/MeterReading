using System.Threading.Tasks;
using Ensek.MeterReading.Api.Data.Context;
using Ensek.MeterReading.Api.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ensek.MeterReading.Api.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly EnsekMeterReadingDbContext _dbContext;
        private ILogger<AccountRepository> _logger;

        public AccountRepository(EnsekMeterReadingDbContext dbContext,ILogger<AccountRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task<Account> GetAccount(int accountId)
        {
            return await _dbContext.Account.FirstOrDefaultAsync(a => a.AccountId == accountId);
        }
    }
}