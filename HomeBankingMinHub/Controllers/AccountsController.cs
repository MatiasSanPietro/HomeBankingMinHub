using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static HomeBankingMinHub.Services.AccountService;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("accounts")]
        public IActionResult Get()
        {
            try
            {
                var accountsDTO = _accountService.GetAllAccounts();
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
                var accountDTO = _accountService.GetAccountById(id);
                return Ok(accountDTO);
            }
            catch (AccountServiceException ex)
            {
                return StatusCode(403, ex.Message);
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
                var accountDTO = _accountService.GetCurrentAccounts(email);
                return Ok(accountDTO);
            }
            catch (AccountServiceException ex)
            {
                return StatusCode(403, ex.Message);

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
                AccountCreateDTO newAccountDTO = _accountService.CreateAccount(email);
                return Created("", newAccountDTO);
            }
            catch (AccountServiceException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}