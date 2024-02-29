using ClientService.RemoteModel;
using ClientService.RemoteService;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionRemoteController : ControllerBase
    {
        private TransactionService _transactionService;

        public TransactionRemoteController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpGet("{clientId}")]
        public async Task<List<TransactionRemote>> Get(long clientId)
        {
            try
            {
                var response = await _transactionService.GetTransaction(clientId);
                if (response.resultado)
                {
                    var objetosTransactions = response.transaction;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
