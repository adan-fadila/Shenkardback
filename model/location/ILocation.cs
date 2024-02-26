using Card_package;
using Game_package;
using Player_package;

namespace Location_package
{
    public interface ILocation
    {

        int id { get; set; }
        string Name { get; }
        string Desc { get; }
        Zone zone1 { get; }
        Zone zone2 { get; }
        string Image {get;set;}
        ILocationBattleStrategy battleStrategy { get; }
        public void setPlayer(int id1,int id2);
        void applyEffect(Game game);
        void playCard(Player player,ICard card);
    }
}