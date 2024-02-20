using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;

namespace HomeBankingMinHub.Services.Interfaces
{
    public interface ILoanService
    {
        List<LoanDTO> GetAllLoans();
        ClientLoan ApplyForLoan(string email, LoanApplicationDTO loanApplicationDTO);
    }
}
