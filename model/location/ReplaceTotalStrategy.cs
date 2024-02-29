using Game_package;

namespace Location_package
{
    class ReplaceTotalStrategy : ILocationBattleStrategy
    {
        public void activate(Game game, Location location)
        {
            Console.WriteLine("ReplaceTotalStrategy");
           int tmp = location.zone1.total;
           location.zone1.total = location.zone2.total;
           location.zone2.total = tmp;
        }
    }
}