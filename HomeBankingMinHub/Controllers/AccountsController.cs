using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
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
                    return StatusCode(403, "La cuenta no existe");
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
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
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
                    return StatusCode(403, "No hay clientes logeados");
                }

                Client client = _clientRepository.FindByEmail(email);
                
                if (client == null)
                {
                    return StatusCode(403, "El cliente no existe");
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
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPost("clients/current/accounts")]
        public IActionResult Post()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                
                if (string.IsNullOrEmpty(email))
                {
                    return StatusCode(403, "No hay clientes logeados");
                }

                Client client = _clientRepository.FindByEmail(email);
               
                if (client == null)
                {
                    return StatusCode(403, "El cliente no existe");
                }

                if (client.Accounts.Count > 2)
                {
                    return StatusCode(403, "El cliente ha alcanzado el limite de cuentas");
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

                return Created("", newAccountDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}