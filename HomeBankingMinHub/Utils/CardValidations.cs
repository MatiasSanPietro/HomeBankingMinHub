using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Utils
{
    public class CardValidations
    {
        // Cuento los tipos de tarjeta, veo que tipo de tarjeta quiero agregar,
        // checkeo si la cantidad del tipo de tarjeta es mayor a 2
        public static bool CheckCardTypeLimit(Client client, CardDTO card)
        {
            int debitCardCount = client.Cards.Count(c => c.Type == CardType.DEBIT);
            int creditCardCount = client.Cards.Count(c => c.Type == CardType.CREDIT);

            CardType newCardType = Enum.Parse<CardType>(card.Type);

            return (newCardType == CardType.DEBIT && debitCardCount > 2) ||
                   (newCardType == CardType.CREDIT && creditCardCount > 2);
        }

        // Verifico en todas las tarjetas del cliente si la nueva tarjeta no tiene
        // el mismo color y tipo
        public static bool CheckUniqueColorPerCardType(Client client, CardDTO card)
        {
            CardType newCardType = Enum.Parse<CardType>(card.Type);
            CardColor newCardColor = Enum.Parse<CardColor>(card.Color);

            return client.Cards.All(c => c.Type != newCardType || c.Color != newCardColor);
        }

        public static int GenerateCVV()
        {
            Random random = new Random();
            return random.Next(100, 1000);
        }
    }
}
