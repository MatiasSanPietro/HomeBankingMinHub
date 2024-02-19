using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Repositories.Interfaces;
using HomeBankingMinHub.Services.Interfaces;

namespace HomeBankingMinHub.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IClientRepository _clientRepository;

        public AccountService(IAccountRepository accountRepository, IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }

        public List<AccountDTO> GetAllAccounts()
        {
            var accounts = _accountRepository.GetAllAccounts();
            var accountsDTO = accounts.Select(account => new AccountDTO
            {
                Id = account.Id,
                Number = account.Number,
                CreationDate = account.CreationDate,
                Balance = account.Balance,
                Transactions = account.Transactions.Select(tr => new TransactionDTO
                {
                    Id = tr.Id,
                    Type = tr.Type.ToString(),
                    Amount = tr.Amount,
                    Description = tr.Description,
                    Date = tr.Date
                }).ToList()
            }).ToList();

            return accountsDTO;
        }

        public AccountDTO GetAccountById(long id)
        {
            var account = _accountRepository.FindById(id);

            if (account == null)
            {
                return null;
            }

            var accountDTO = new AccountDTO
            {
                Id = account.Id,
                Number = account.Number,
                CreationDate = account.CreationDate,
                Balance = account.Balance,
                Transactions = account.Transactions.Select(tr => new TransactionDTO
                {
                    Id = tr.Id,
                    Type = tr.Type.ToString(),
                    Amount = tr.Amount,
                    Description = tr.Description,
                    Date = tr.Date
                }).ToList()
            };

            return accountDTO;
        }

        public IEnumerable<AccountDTO> GetCurrentAccounts(string email)
        {
            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                return null;
            }

            var accountDTO = client.Accounts.Select(ac => new AccountDTO
            {
                Id = ac.Id,
                Balance = ac.Balance,
                CreationDate = ac.CreationDate,
                Number = ac.Number
            }).ToList();

            return accountDTO;
        }

        public AccountCreateDTO CreateAccount(string email)
        {
            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                return null;
            }

            if (client.Accounts.Count > 2)
            {
                return null;
            }

            Account newAccount = new Account
            {
                CreationDate = DateTime.Now,
                Balance = 0,
                ClientId = client.Id,
            };

            _accountRepository.Save(newAccount);

            AccountCreateDTO newAccountDTO = new AccountCreateDTO
            {
                CreationDate = newAccount.CreationDate,
                Balance = 0,
            };

            return newAccountDTO;
        }
    }
}