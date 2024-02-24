using System.Text;

namespace HomeBankingMinHub.Utils
{
    public class CardHandler
    {
        public static string GenerateCardNumber()
        {
            var random = new Random();
            var cardNumber = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                cardNumber.Append(random.Next(10000).ToString("D4"));
                if (i < 3)
                {
                    cardNumber.Append('-');
                }
            }
            return cardNumber.ToString();
        }
        public static int GenerateCVV()
        {
            var random = new Random();
            return random.Next(100, 1000);
        }
    }
}
