
/*************************************/
/*JUST FOR TESTING*/

using Location_package;
using Player_package;

namespace Card_package
{
    public class SimpleAbilityStrategy : IAbilityStrategy
    {
        public SimpleAbilityStrategy()
        {
        }
        public void ActivateAbility(Zone playerZone, Player player)
        {
            Console.WriteLine("SimpleAbilityStrategy");
            playerZone.total *= 2;
        }


    }
}
