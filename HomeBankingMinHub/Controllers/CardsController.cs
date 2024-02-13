using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace HomeBankingMinHub.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private ICardRepository _cardRepository;

        public CardsController(IClientRepository clientRepository, ICardRepository cardRepository)
        {
            _clientRepository = clientRepository;
            _cardRepository = cardRepository;
        }

        [HttpGet("clients/current/cards")]
        public IActionResult GetCurrentCards()
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "No hay clientes logeados");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
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

                return Ok(cardDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        [HttpPost("clients/current/cards")]
        public IActionResult Post([FromBody] CardDTO card)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "No hay clientes logeados");
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                // Cuento los tipos de tarjeta, veo que tipo de tarjeta quiero agregar,
                // checkeo si la cantidad del tipo de tarjeta es mayor a 2
                bool CheckCardTypeLimit(Client client, CardDTO card)
                {
                    int debitCardCount = client.Cards.Count(c => c.Type == CardType.DEBIT);
                    int creditCardCount = client.Cards.Count(c => c.Type == CardType.CREDIT);

                    CardType newCardType = Enum.Parse<CardType>(card.Type);

                    return (newCardType == CardType.DEBIT && debitCardCount > 2) ||
                           (newCardType == CardType.CREDIT && creditCardCount > 2);
                }

                if (CheckCardTypeLimit(client, card))
                {
                    return StatusCode(403, "El cliente ha alcanzado el límite de tarjetas para este tipo");
                }

                string GenerateCardNumber()
                {
                    Random random = new();
                    StringBuilder cardNumber = new StringBuilder();

                    for (int i = 0; i < 4; i++)
                    {
                        cardNumber.AppendFormat("{0:D4} ", random.Next(0, 10000));
                    }

                    return cardNumber.ToString().Trim();
                }

                string numCard = GenerateCardNumber();

                int GenerateCVV()
                {
                    Random random = new();
                    return random.Next(100, 1000);
                }

                int cvvNum = GenerateCVV();

                Card newCard = new Card()
                {
                    ClientId = client.Id,
                    CardHolder = $"{client.FirstName} {client.LastName}",
                    Type = Enum.Parse<CardType>(card.Type),
                    Color = Enum.Parse<CardColor>(card.Color),
                    Number = numCard,
                    Cvv = cvvNum,
                    FromDate = DateTime.Now,
                    ThruDate = DateTime.Now.AddYears(5),
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

                return Created("", cardDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
