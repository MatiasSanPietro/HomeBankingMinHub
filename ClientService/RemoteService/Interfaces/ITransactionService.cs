using ClientService.RemoteModel;

namespace ClientService.RemoteService.Interfaces
{
    public interface ITransactionService
    {
        Task<(bool resultado, TransactionRemote transaction, string errorMessage)> GetTransaction(long clientId);
        Task<(bool resultado, string errorMessage)> PostTransaction(TransactionRemote transactionRemote);
    }
}
