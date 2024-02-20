using HomeBankingMinHub.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using HomeBankingMinHub.Services.Interfaces;
using static HomeBankingMinHub.Services.TransactionService;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] TransferDTO transferDTO)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (string.IsNullOrEmpty(email))
                {
                    return StatusCode(403, "No hay clientes logeados");
                }

                _transactionService.MakeTransfer(email, transferDTO);

                return Ok("Transferencia realizada con exito");
            }
            catch (TransactionServiceException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}