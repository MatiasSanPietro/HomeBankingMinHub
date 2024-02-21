namespace HomeBankingMinHub.Models.DTOs
{
    public class ClientLoanDTO
    {
        public long Id { get; set; }
        public long LoanId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Payments { get; set; }

        public ClientLoanDTO(ClientLoan loan)
        {
            Id = loan.Id;
            LoanId = loan.LoanId;
            Amount = loan.Amount;
            Name = loan.Loan.Name;
            Payments = int.Parse(loan.Payments);
        }
    }
}
