using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;

namespace HomeBankingMinHub.Services.Interfaces
{
    public interface IAccountService
    {
        List<AccountDTO> GetAllAccounts();
        AccountDTO GetAccountById(long id);
        IEnumerable<AccountDTO> GetCurrentAccounts(string email);
        AccountCreateDTO CreateAccount(string email);
    }
}