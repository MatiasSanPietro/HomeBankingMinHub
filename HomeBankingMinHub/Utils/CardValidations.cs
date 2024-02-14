using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Utils
{
    public class CardValidations
    {
        // Cuento los tipos de tarjeta, veo que tipo de tarjeta quiero agregar,
        // checkeo si la cantidad del tipo de tarjeta es mayor a 2
        // Despues agregar que no se puedan repetir colores
        public static bool CheckCardTypeLimit(Client client, CardDTO card)
        {
            int debitCardCount = client.Cards.Count(c => c.Type == CardType.DEBIT);
            int creditCardCount = client.Cards.Count(c => c.Type == CardType.CREDIT);

            CardType newCardType = Enum.Parse<CardType>(card.Type);

            return (newCardType == CardType.DEBIT && debitCardCount > 2) ||
                   (newCardType == CardType.CREDIT && creditCardCount > 2);
        }
        public static int GenerateCVV()
        {
            Random random = new Random();
            return random.Next(100, 1000);
        }
    }
}
