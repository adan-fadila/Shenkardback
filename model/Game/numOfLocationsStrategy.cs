using System.Collections.Generic;
using System.Linq;
using Location_package;
using Player_package;

namespace Game_package
{
    public class numOfLocationsStrategy : IBattleStrategy
    {

        public List<int> battle(Game game)
        {
            IDictionary<int, int> playersNumOfwinning = new Dictionary<int, int>();
            foreach (Location l in game.locations)
            {
                int p = getWinningPlayer(l);
                if(p == -1){
                    continue;
                }
                playersNumOfwinning[p]++;
            }
            int maxWins = playersNumOfwinning.Values.Max();
            List<int> winner = new List<int>();
            // Finding the player(s) with the maximum wins
            foreach (int p in playersNumOfwinning.Keys)
            {
                if (playersNumOfwinning[p] == maxWins)
                {
                    winner.Add(p);
                }
            }
            return winner;
        }
        public int getWinningPlayer(Location location)
        {
            if (location.zone1.total == location.zone2.total)
            {
                return -1;
            }
            return (location.zone1.total > location.zone2.total) ? location.zone1.Player : location.zone2.Player;
        }
    }
}