using System.Collections.Generic;
using Player_package;

namespace Game_package
{
    public interface IBattleStrategy
    {
        public List<int> battle(Game game);
    }
}
