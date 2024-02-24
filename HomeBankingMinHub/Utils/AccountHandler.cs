namespace HomeBankingMinHub.Utils
{
    public class AccountHandler
    {
        public static string GenerateAccountNumber()
        {
            var random = new Random();
            string number = random.Next(100000000).ToString("D8");

            return $"VIN-{number}";
        }
    }
}
