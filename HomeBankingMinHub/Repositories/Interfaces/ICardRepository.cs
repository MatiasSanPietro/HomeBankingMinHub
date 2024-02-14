using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Repositories.Interfaces
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards();
        Card FindById(long id);
        void Save(Card card);
        IEnumerable<Card> GetCardsByClient(long clientId);
        Card FindByNum(string number);
    }
}
