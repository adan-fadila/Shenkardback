using System.Collections.Generic;
using Card_package;
using Game_package;
using Location_package;
using Player_package;

class GameController
{
    private Game game;
    private GameService gameService = GameService.getInstance();
    
    public void putCardToLocation(Player player, Location location, ICard card)
    {
        gameService.putCardToLocation(player, location, card, game);

    }

    // public void endTurn()
    public Game askForGame(int id1,int id2)
    {
        this.game = gameService.askForGame(id1,id2);
        return game;
    }
    public List<int> startBattle()
    {
        return gameService.startBattle(game);
    }
    public void SetWaitingPlayer(int playerId)
    {
        gameService.SetWaitingPlayer(playerId);
    }
     public bool IsWaitingPlayerAvailable(){
        return gameService.IsWaitingPlayerAvailable();
     }
}