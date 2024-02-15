using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories.Interfaces
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAllLoans();
        Loan FindById(long id);
    }
}
