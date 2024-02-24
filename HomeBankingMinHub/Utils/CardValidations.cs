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

            var newCardType = Enum.Parse<CardType>(card.Type);

            return (newCardType == CardType.DEBIT && debitCardCount > 2) ||
                   (newCardType == CardType.CREDIT && creditCardCount > 2);
        }

        // Verifico si alguna de las cartas del cliente tiene el mismo tipo y color de la nueva carta
        public static bool CheckCardTypeAndColor(Client client, CardDTO card)
        {
            var newCardType = Enum.Parse<CardType>(card.Type);
            var newCardColor = Enum.Parse<CardColor>(card.Color);

            return client.Cards.Any(c => c.Type == newCardType && c.Color == newCardColor);
        }
    }
}
