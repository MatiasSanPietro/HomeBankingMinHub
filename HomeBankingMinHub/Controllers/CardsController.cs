using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
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
                    return Forbid();
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
                return StatusCode(500, ex.Message);
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
                    return Forbid();
                }

                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid();
                }

                if (client.Cards.Count > 2)
                {
                    return StatusCode(403, "Un cliente no puede tener mas de 3 tarjetas");
                }

                string GenerateCardNumber()
                {
                    Random random = new();
                    StringBuilder cardNumber = new StringBuilder();

                    for (int i = 0; i < 4; i++)
                    {
                        //string section = random.Next(0, 10000).ToString("D4");
                        //cardNumber.Append(section + " ");
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
                return StatusCode(500, ex.Message);
            }
        }
    }
}
