
/*************************************/
/*JUST FOR TESTING*/

using Location_package;
using Player_package;

namespace Card_package
{
    public class AnotherAbilityStrategy : IAbilityStrategy
    {
        public AnotherAbilityStrategy()
        {
        }
        public void ActivateAbility(Zone playerZone, Player player)
        {
            Console.WriteLine("AnotherAbilityStrategy");
            player.energy *= 2;
        }


    }
}
