using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories.Interfaces;
using HomeBankingMinHub.Utils;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Account FindById(long id)
        {
            return FindByCondition(account => account.Id == id)
                .Include(account => account.Transactions)
                .FirstOrDefault();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
                .Include(account => account.Transactions)
                .ToList();
        }

        public IEnumerable<Account> GetAccountsByClient(long clientId)

        {

            return FindByCondition(account => account.ClientId == clientId)
                .Include(account => account.Transactions)
                .ToList();
        }
        public Account FindByVIN(string VIN)
        {
            return FindByCondition(acc => acc.Number == VIN)
                .Include(acc => acc.Transactions)
                .FirstOrDefault();
        }

        public void Save(Account account)
        {
            bool condition = true;
            string vin = string.Empty;

            while (condition)
            {
                vin = AccountHandler.GenerateAccountNumber();

                var acc = FindByVIN(vin);

                if (acc == null)
                {
                    condition = false;
                }
            }
            account.Number = vin;
            Create(account);
            SaveChanges();
        }
    }
}
