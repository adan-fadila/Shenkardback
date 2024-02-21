using System.Collections.Generic;
using Card_package;
using Game_package;
using Location_package;
using Player_package;

class GameController
{
    private GameService gameService = GameService.getInstance();

    public Game putCardToLocation(Player player, ILocation location, ICard card, Game game)
    {
        return gameService.putCardToLocation(player, location, card, game);

    }


    public void endTurn(Game game)
    {
        game.endTurn();
    }
    public Game askForGame(int id1, int id2)
    {
        return gameService.askForGame(id1, id2);

    }
    public List<int> startBattle(Game game)
    {
        return gameService.startBattle(game);
    }

}