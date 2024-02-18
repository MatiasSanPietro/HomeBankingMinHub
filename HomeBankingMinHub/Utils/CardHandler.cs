using System.Text;

namespace HomeBankingMinHub.Utils
{
    public class CardHandler
    {
        public static string GenerateCardNumber()
        {
            Random random = new();
            StringBuilder cardNumber = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                cardNumber.AppendFormat("{0:D4}", random.Next(0, 10000));
                if (i < 3)
                {
                    cardNumber.Append('-');
                }
            }
            return cardNumber.ToString().Trim();
        }
        public static int GenerateCVV()
        {
            Random random = new Random();
            return random.Next(100, 1000);
        }
    }
}
