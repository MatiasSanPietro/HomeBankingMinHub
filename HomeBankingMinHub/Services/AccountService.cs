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

        // Excepcion personalizada
        public class AccountServiceException : Exception
        {
            public AccountServiceException(string message) : base(message)
            {
            }
        }

        public List<AccountDTO> GetAllAccounts()
        {
            var accounts = _accountRepository.GetAllAccounts();
            List<AccountDTO> accountsDTO = new List<AccountDTO>();

            foreach (var account in accounts)
            {
                accountsDTO.Add(new AccountDTO(account));
            }

            return accountsDTO;
        }

        public AccountDTO GetAccountById(long id)
        {
            var account = _accountRepository.FindById(id);

            if (account == null)
            {
                throw new AccountServiceException("La cuenta no existe");
            }

            var accountDTO = new AccountDTO(account);

            return accountDTO;
        }

        public IEnumerable<AccountDTO> GetCurrentAccounts(string email)
        {
            var client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                throw new AccountServiceException("El cliente no existe");
            }
            // aca
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
            var client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                throw new AccountServiceException("El cliente no existe");
            }

            if (client.Accounts.Count > 2)
            {
                throw new AccountServiceException("El cliente ha alcanzado el limite de cuentas");
            }

            Account newAccount = new()
            {
                CreationDate = DateTime.Now,
                Balance = 0,
                ClientId = client.Id,
            };

            _accountRepository.Save(newAccount);

            AccountCreateDTO newAccountDTO = new()
            {
                CreationDate = newAccount.CreationDate,
                Balance = 0,
            };

            return newAccountDTO;
        }
    }
}