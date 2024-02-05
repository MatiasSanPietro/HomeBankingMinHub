using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        Account FindById(long id);
        void Save(Account account);
    }
}
