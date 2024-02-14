namespace HomeBankingMinHub.Utils
{
    public class AccountHandler
    {
        public static string GenerateAccountNumber()
        {
            Random random = new Random();
            string number = random.Next(0, 100000000).ToString("D8");

            string result = $"VIN-{number}";
            return result;
        }
    }
}
