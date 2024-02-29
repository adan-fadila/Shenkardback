using Game_package;

namespace Location_package
{
    class NegativeTotalStrategy : ILocationBattleStrategy
    {
        public void activate(Game game, Location location)
        {
            Console.WriteLine("NegativeTotalStrategy");
            location.zone2.total *= -1;
            location.zone1.total *= -1;
        }
    }
}