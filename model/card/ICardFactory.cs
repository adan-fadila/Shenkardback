using System.Collections;
using System.Collections.Generic;

namespace Card_package
{
    public interface ICardFactory
    {
        public ICard generate(int id);
    }

}