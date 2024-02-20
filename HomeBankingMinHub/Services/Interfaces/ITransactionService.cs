using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;

namespace HomeBankingMinHub.Services.Interfaces
{
    public interface ITransactionService
    {
        void MakeTransfer(string email, TransferDTO transferDTO);
    }
}
