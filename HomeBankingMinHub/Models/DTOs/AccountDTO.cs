using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMinHub.Models.DTOs
{
    public class AccountDTO
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
        public ICollection<TransactionDTO> Transactions { get; set; }

        //public AccountDTO(Account account)
        //{
        //    MapFromAccount(account);
        //}

        //public void MapFromAccount(Account account)
        //{
        //    Id = account.Id;
        //    Number = account.Number;
        //    CreationDate = account.CreationDate;
        //    Balance = account.Balance;
        //    Transactions = account.Transactions.Select(tr => new TransactionDTO
        //    {
        //        Id = tr.Id,
        //        Type = tr.Type.ToString(),
        //        Amount = tr.Amount,
        //        Description = tr.Description,
        //        Date = tr.Date
        //    }).ToList();
        //}
    }
}
