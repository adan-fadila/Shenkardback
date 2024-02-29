using Card_package;
using Game_package;

namespace Location_package
{
    class ReplaceCardStrategy : ILocationBattleStrategy
    {
        public void activate(Game game, Location location)
        {
            Console.WriteLine("ReplaceCardStrategy");
            if (location.zone1.GetCards().Count != 0)
            {
                Console.WriteLine("zone1 have cards");
                ICard card1 = location.zone1.GetLastCard();
                location.zone2.setCard(card1);
            }
            if (location.zone2.GetCards().Count != 0)
            {
                Console.WriteLine("zone2 have cards");
                ICard card2 = location.zone2.GetLastCard();
                location.zone1.setCard(card2);
            }


        }
    }
}