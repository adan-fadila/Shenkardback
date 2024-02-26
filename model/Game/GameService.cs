using System;
using System.Collections.Generic;
using System.Diagnostics;
using Card_package;
using Location_package;
using Player_package;

namespace Game_package
{
    public class GameService
    {

        private static GameService? instance;
        private static int numOfPlayerHandCardsInGameStart = 3;
        private PlayerService playerService = PlayerService.getInstance();
        private LocationService locationService = LocationService.getInstance();


        private GameService()
        {

        }
        public static GameService getInstance()
        {
            if (instance == null)
            {
                instance = new GameService();
            }
            return instance;
        }
        public Game putCardToLocation(Player player, ILocation location, ICard card, Game game)
        {
            locationService.putCardToLocation(player, location, card, game);
            return game;

        }
        public bool endTurn(Game game)
        {
            foreach (Player player in game.Players)
            {
                drawCard(game,1);
                playerService.updatePlayerEnergy(player, 1);
            }
            return game.endTurn();
        }
     
        public Game askForGame(int Player1ID, int Player2ID)
        {

            Player player1 = playerService.GetPlayer(Player1ID);
            Player player2 = playerService.GetPlayer(Player2ID);
            Location[] locations = GetRandomLocations(3);
            return initGame(player1, player2, new numOfLocationsStrategy(), locations);
        }
        private Game initGame(Player player1, Player player2, IBattleStrategy battleStrategy, Location[] locations)
        {
            Game game = new Game(player1, player2, battleStrategy, locations);
            drawCard(game, numOfPlayerHandCardsInGameStart);

            return game;
        }
        private void drawCard(Game game, int num)
        {
            foreach (Player p in game.Players)
            {
                playerService.drawCard(p, num);
            }
        }

        public List<int> startBattle(Game game)
        {
            return game.getWinner();
        }


        private Location[] GetRandomLocations(int numOflocations)
        {
            return locationService.getLocations(numOflocations);
        }


    }
}