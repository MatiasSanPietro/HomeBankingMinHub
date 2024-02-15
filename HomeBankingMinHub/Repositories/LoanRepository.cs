using HomeBankingMindHub.Repositories;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories.Interfaces;

namespace HomeBankingMinHub.Repositories
{
    public class LoanRepository : RepositoryBase<Loan>, ILoanRepository
    {
        public LoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }
        public IEnumerable<Loan> GetAllLoans()
        {
            return FindAll()
                .ToList();
        }
        public Loan FindById(long id)
        {
            return FindByCondition(loan => loan.Id == id)
                .FirstOrDefault();
        }
    }
}
