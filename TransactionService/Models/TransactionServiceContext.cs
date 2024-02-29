using Microsoft.EntityFrameworkCore;

namespace TransactionService.Models
{
    public class TransactionServiceContext : DbContext
    {
        public TransactionServiceContext(DbContextOptions<TransactionServiceContext> options) : base(options) { }
        public DbSet<Transaction> Transactions { get; set;}
    }
}
