using HomeBankingMinHub.Models;
using HomeBankingMinHub.Models.DTOs;

namespace HomeBankingMinHub.Services.Interfaces
{
    public interface ICardService
    {
        List<CardDTO> GetCurrentCards(string email);
        CardDTO CreateCard(string email, CardDTO card);
    }
}
