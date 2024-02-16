using HomeBankingMinHub.Models.DTOs;
using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Utils.Interfaces
{
    public interface ICardValidations
    {
        public bool CheckCardTypeLimit(Client client, CardDTO card);
        public bool CheckUniqueColorPerCardType(Client client, CardDTO card);
        public int GenerateCVV();
    }
}
