using System.ComponentModel.DataAnnotations;

namespace HomeBankingMinHub.Models.DTOs
{
    public class LoanApplicationDTO
    {
        [Key]
        public long LoanId { get; set; }
        public double Amount { get; set; }
        public string Payments { get; set; }
        public string ToAccountNumber { get; set; }
    }
}
