using TransactionService.Models;

namespace TransactionService.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        void Save(Transaction transaction);
        IEnumerable<Transaction> GetUsers(long clientId);
    }
}
