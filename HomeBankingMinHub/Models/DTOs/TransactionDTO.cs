namespace HomeBankingMinHub.Models.DTOs
{
    public class TransactionDTO
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public TransactionDTO(Transaction transaction)
        {
            Id = transaction.Id;
            Type = transaction.Type.ToString();
            Description = transaction.Description;
            Date = transaction.Date;
            Amount = transaction.Amount;
        }
    }
}
