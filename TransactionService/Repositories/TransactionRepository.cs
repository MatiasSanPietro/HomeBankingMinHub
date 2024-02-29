using TransactionService.Models;
using TransactionService.Repositories.Interfaces;

namespace TransactionService.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(TransactionServiceContext repositoryContext) : base(repositoryContext)
        {
        }
        public IEnumerable<Transaction> GetUsers(long clientId)
        {
            return FindByCondition(t=>t.ClientId == clientId)
                .ToList();
        }

        public void Save(Transaction transaction)
        {
            Create(transaction);
            SaveChanges();
        }
    }
}
