using HomeBankingMindHub.Repositories;
using HomeBankingMinHub.Models;
using HomeBankingMinHub.Repositories.Interfaces;
using HomeBankingMinHub.Utils;

namespace HomeBankingMinHub.Repositories
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }

        public Card FindById(long id)
        {
            return FindByCondition(card => card.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Card> GetAllCards()
        {
            return FindAll()
                .ToList();
        }

        public IEnumerable<Card> GetCardsByClient(long clientId)
        {
            return FindByCondition(card => card.ClientId == clientId)
                .ToList();
        }

        public Card FindByNum(string number)
        {
            return FindByCondition(card => card.Number == number)
                .FirstOrDefault();
        }

        public void Save(Card card)
        {
            string num;

            do
            {
                num = CardHandler.GenerateCardNumber();
            } while (FindByNum(num) != null);

            card.Number = num;

            Create(card);
            SaveChanges();
        }
    }
}
