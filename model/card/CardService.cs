
using System.Collections.Generic;
namespace Card_package
{
    public class CardService
    {
        private readonly cardDataAccess cardData = cardDataAccess.getInstance();
        private readonly CardFactory cardFactory = new CardFactory();
        private static CardService instance;
        public readonly CardService cardService;
        private CardService()
        {
        }
        public static CardService getInstance()
        {
            if (instance == null)
            {
                instance = new CardService();
            }
            return instance;
        }
        public void createCard(string name, string desc, int cost, int power)
        {
            cardData.addRegularCard(name, desc, cost, power);
        }

        public void deleteCard(int id)
        {
            cardData.deleteCard(id);
        }

        public ICard getCard(int id)
        {
            return cardFactory.generate(id);
        }


        public List<ICard> getCards()
        {
            List<ICard> allCards = new List<ICard>();
            List<int> ids = cardData.getAllCards();
            for (int i = 0; i < ids.Count; i++)
            {
                allCards.Add(cardFactory.generate(ids[i]));
            }
            return allCards;
        }

        public ICard[] getRandomCards(int regular, int ability, int master)
        {
            int numOfCards = regular + ability + master;
            int[] cardsId = cardData.getRandomCards(regular, master, ability);
            ICard[] defaultDeck = new ICard[regular + ability + master];
            for (int i = 0; i < numOfCards; i++)
            {
                defaultDeck[i] = cardFactory.generate(cardsId[i]);
            }
            return defaultDeck;
        }

        public void updateCard(int id, int cost, int power)
        {
            cardData.updateCard(id, cost, power);
        }
        public string getCardType(int id)
        {
            return cardData.getCardType(id);
        }
        public string getCardName(int id)
        {
            return cardData.getCardName(id);
        }


    }
}
