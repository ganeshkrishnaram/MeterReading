using System.Threading.Tasks;
using Ensek.MeterReading.Api.Data.Model;

namespace Ensek.MeterReading.Api.Repositories
{
    public interface IAccountRepository
    {
        public Task<Account> GetAccount(int accountId);
    }
}