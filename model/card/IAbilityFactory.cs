using System.Collections;
using System.Collections.Generic;

namespace Card_package
{
   public interface IAbilityFactory {
   public IAbilityStrategy generate(string name); 
}

}
