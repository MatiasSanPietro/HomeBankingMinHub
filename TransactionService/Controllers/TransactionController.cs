using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Repositories.Interfaces;

namespace TransactionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpGet("{clientId}")]
        public IActionResult Get(long clientId) 
        {
            try
            {
                var transactions = _transactionRepository.GetUsers(clientId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
