using System.Collections.Generic;
using Card_package;
using Game_package;
using Location_package;
using Player_package;

class GameController
{
    private GameService gameService = GameService.getInstance();

    public Game putCardToLocation(Player player, int locationId, int cardId, Game game)
    {
        

        ILocation? location = getLocationById(game, locationId);
        ICard? card = getCardById(cardId, player);
        if (card == null || location == null)
        {
            return null;
        }

        return gameService.putCardToLocation(player, location, card, game);

    }
    public Game ApplyLocationEffect(Game game, int locationId){
         ILocation? location = getLocationById(game, locationId);
        
        if ( location == null)
        {
            return null;
        }

        return gameService.ApplyLocationEffect(game,location);
    }

    private ILocation getLocationById(Game game, int id)
    {
        foreach (ILocation location in game.locations)
        {
            Console.WriteLine($"id: {id}, locationId {location.id}");   
            if (location.id == id)
            {
                return location;
            }
        }
        Console.WriteLine("notfound");
        return null;
    }
    private ICard getCardById(int id, Player player)
    {
        foreach (ICard card in player.displayedCards)
        {
               
            if (card.id == id)
            {
            
                return card;
            }
        }
        
        return null;
    }

    public bool endTurn(Game game)
    {
        
        return gameService.endTurn(game);
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