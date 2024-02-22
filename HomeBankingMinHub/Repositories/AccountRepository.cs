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
        public Account FindByNumber(string number)
        {
            return FindByCondition(acc => acc.Number.ToUpper() == number.ToUpper())
                .Include(acc => acc.Transactions)
                .FirstOrDefault();
        }

        public void Save(Account account)
        {
            bool condition = true; // aca
            string vin = string.Empty;

            if (account.Id == 0)
            {
                while (condition)
                {
                    vin = AccountHandler.GenerateAccountNumber();

                    var acc = FindByNumber(vin);

                    if (acc == null)
                    {
                        condition = false;
                    }
                }
                account.Number = vin;
            }

            if (account.Id == 0)
            {
                Create(account);
            }
            else
            {
                Update(account);
            }
            SaveChanges();
        }
    }
}
