using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Repositories.Interfaces;
using HomeBankingMinHub.Services.Interfaces;
using HomeBankingMinHub.Utils;

namespace HomeBankingMinHub.Services
{
    public class CardService : ICardService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ICardRepository _cardRepository;

        public CardService(IClientRepository clientRepository, ICardRepository cardRepository)
        {
            _clientRepository = clientRepository;
            _cardRepository = cardRepository;
        }

        public class CardServiceException : Exception
        {
            public CardServiceException(string message) : base(message)
            {
            }
        }

        public List<CardDTO> GetCurrentCards(string email)
        {
            var client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                throw new CardServiceException("El cliente no existe");
            }

            List<CardDTO> cardDTO = new List<CardDTO>();

            foreach (var card in client.Cards)
            {
                cardDTO.Add(new CardDTO(card));
            }

            return cardDTO;
        }

        public CardDTO CreateCard(string email, CardDTO card)
        {
            var client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                throw new CardServiceException("El cliente no existe");
            }

            if (CardValidations.CheckCardTypeLimit(client, card))
            {
                throw new CardServiceException("El cliente ha alcanzado el límite de tarjetas para este tipo");
            }

            if (CardValidations.CheckCardTypeAndColor(client, card))
            {
                throw new CardServiceException("Ya existe una tarjeta con el mismo tipo y color");
            }

            int cvvNum = CardHandler.GenerateCVV();

            var newCard = new Card()
            {
                ClientId = client.Id,
                CardHolder = $"{client.FirstName} {client.LastName}",
                Type = Enum.Parse<CardType>(card.Type),
                Color = Enum.Parse<CardColor>(card.Color),
                Cvv = cvvNum,
                FromDate = DateTime.Now,
                ThruDate = (card.Type == CardType.DEBIT.ToString()) ? DateTime.Now.AddYears(4) : DateTime.Now.AddYears(5),
            };

            _cardRepository.Save(newCard);

            var cardDTO = new CardDTO(newCard);

            return cardDTO;
        }
    }
}
