using HomeBankingMindHub.Repositories;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        private IClientRepository _clientRepository;

        public AccountsController(IAccountRepository accountRepository, IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }

        [HttpGet("accounts")]
        public IActionResult Get()
        {
            try
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

                return Ok(accountsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("accounts/{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var account = _accountRepository.FindById(id);
                if (account == null)
                {
                    return Forbid();
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

                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("clients/current/accounts")]
        public IActionResult GetCurrentAccounts()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                var accountDTO = client.Accounts.Select(ac => new AccountDTO
                {
                    Id = ac.Id,
                    Balance = ac.Balance,
                    CreationDate = ac.CreationDate,
                    Number = ac.Number
                }).ToList();

                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("clients/current/accounts")]
        public IActionResult Post()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid();
                }

                // Buscamos si ya existe el usuario
                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                if (client.Accounts.Count > 2)
                {
                    return StatusCode(403, "Un cliente no puede tener mas de 3 cuentas");
                }

                string GenerateRandomNumber()
                {
                    Random random = new Random();
                    return random.Next(0, 100000000).ToString("D8");
                }

                string randomNum = GenerateRandomNumber();

                Account newAccount = new Account
                {
                    Number = $"VIN-{randomNum}",
                    CreationDate = DateTime.Now,
                    Balance = 0,
                    ClientId = client.Id,
                };

                _accountRepository.Save(newAccount);

                // Preguntarle al profesor si es necesario hacer un AccountCreateDTO o si es mejor usar el AccountDTO
                AccountCreateDTO newAccountDTO = new AccountCreateDTO
                {
                    Number = newAccount.Number,
                    CreationDate = newAccount.CreationDate,
                    Balance = 0,
                };

                return Created("", newAccountDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}