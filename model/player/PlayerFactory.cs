using Card_package;

namespace Player_package
{
    public class PlayerFactory{
        private playerDataAccess playerDataAccess = playerDataAccess.getInstance();
        public Player generate(int id,ICard[] cards){
            string name = playerDataAccess.getPlayerName(id);
            bool hasDeck = playerDataAccess.hasDeck(id);
            return new Player(id,name,hasDeck,cards);
        }
    }
}