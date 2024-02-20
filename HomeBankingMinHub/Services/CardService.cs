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
            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                throw new CardServiceException("El cliente no existe");
            }

            var cardDTO = client.Cards.Select(c => new CardDTO
            {
                Id = c.Id,
                CardHolder = c.CardHolder,
                Color = c.Color.ToString(),
                Cvv = c.Cvv,
                FromDate = c.FromDate,
                Number = c.Number,
                ThruDate = c.ThruDate,
                Type = c.Type.ToString()
            }).ToList();

            return cardDTO;
        }

        public CardDTO CreateCard(string email, CardDTO card)
        {
            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                throw new CardServiceException("El cliente no existe");
            }

            if (CardValidations.CheckCardTypeLimit(client, card))
            {
                throw new CardServiceException("El cliente ha alcanzado el límite de tarjetas para este tipo");
            }

            if (!CardValidations.CheckUniqueColorPerCardType(client, card))
            {
                throw new CardServiceException("Ya existe una tarjeta con el mismo tipo y color");
            }

            int cvvNum = CardHandler.GenerateCVV();

            Card newCard = new Card()
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

            CardDTO cardDTO = new CardDTO()
            {
                CardHolder = newCard.CardHolder,
                Type = newCard.Type.ToString(),
                Color = newCard.Color.ToString(),
                Number = newCard.Number,
                Cvv = newCard.Cvv,
                FromDate = newCard.FromDate,
                ThruDate = newCard.ThruDate
            };

            return cardDTO;
        }
    }
}
