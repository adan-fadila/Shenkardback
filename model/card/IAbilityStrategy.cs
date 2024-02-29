using System.Collections;
using System.Collections.Generic;
using Location_package;
using Player_package;

namespace Card_package
{
    public interface IAbilityStrategy
    {
        public void ActivateAbility(Zone playerZone, Player player);
    }

}