using System.Collections.Generic;
using Card_package;
using Game_package;
namespace Location_package
{
    /*****************************/
    /*NEED TESTING*/
    class LocationReplaceCardsStrategy : ILocationBattleStrategy
    {
        public void activate(Game game, Location locatin)
        {
            // ICard[] cards = new ICard[locatin.zones.Count];
            // for (int i = 0; i < cards.Length; i++)
            // {
            //     cards[i] = locatin.zones[i].GetLastCard();
            //     if (cards[i] != null)
            //     {
            //         locatin.zones[i].updateTotal(-1 * cards[i].Power);

            //     }
            // }
            // for (int i = 0; i < cards.Length; i++)
            // {
            //     if (i == 0)
            //     {
            //         locatin.zones[i].setCard(cards[cards.Length - 1]);
            //     }
            //     else
            //     {
            //         locatin.zones[i].setCard(cards[i - 1]);
            //     }

            // }
        }
    }
}