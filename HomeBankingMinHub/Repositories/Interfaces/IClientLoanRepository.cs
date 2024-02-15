using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories.Interfaces
{
    public interface IClientLoanRepository
    {
        void Save(ClientLoan clientLoan);
    }
}
