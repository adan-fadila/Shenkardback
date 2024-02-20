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

        private static GameService instance;
        private int waitingPlayer;
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
        public void putCardToLocation(Player player, Location location, ICard card, Game game)
        {
            locationService.putCardToLocation(player, location, card, game);
        }
        public bool endTurn(Game game)
        {
            return game.endTurn();
        }
        public void getTurn(Game game)
        {
            foreach (Player player in game.Players)
            {
                playerService.drawCard(player, 3);
                playerService.updatePlayerEnergy(player, 1);
            }
        }
        public Game askForGame(int Player1ID,int Player2ID)
        {
            // if (!IsWaitingPlayerAvailable())
            // {
            //     SetWaitingPlayer(Player1ID);
            //     return null;
            // }

            // int Player2ID = getFirstWaitingPlayer();
            // if(getFirstWaitingPlayer() == Player1ID){
            //     return null;
            // }
            Player player1 = playerService.GetPlayer(Player1ID);
            Player player2 = playerService.GetPlayer(Player2ID);
            Location[] locations = GetRandomLocations(3);
            return initGame(player1, player2, new numOfLocationsStrategy(), locations);
        }
        private Game initGame(Player player1, Player player2, IBattleStrategy battleStrategy, Location[] locations)
        {
            return new Game(player1, player2, battleStrategy, locations);
        }
        private int getFirstWaitingPlayer()
        {
            return waitingPlayer;
        }
        public List<int> startBattle(Game game)
        {
            do
            {
                getTurn(game);
                SimultaneousCardPlacement(game);

            } while (endTurn(game));

            return game.getWinner();
        }

        public void SetWaitingPlayer(int playerId)
        {
            waitingPlayer = playerId;
        }
        public void ResetWaitingPlayer()
        {
            waitingPlayer = 0;
        }
        public bool IsWaitingPlayerAvailable()
        {
            return waitingPlayer != 0;
        }
        private Location[] GetRandomLocations(int numOflocations)
        {
            return locationService.getLocations(numOflocations);
        }
        private void SimultaneousCardPlacement(Game game)
        {
            // Dictionary to store player actions (selected card and location)
            Dictionary<Player, (ICard card, Location location)> playerActions = new Dictionary<Player, (ICard, Location)>();

            // Prompt each player to select a card and a location for that card
            foreach (Player player in game.Players)
            {
                ICard selectedCard = PromptPlayerSelectCard(player);
                Location selectedLocation = PromptPlayerSelectLocation(player, game.locations);

                // Store the player's selected card and location
                playerActions[player] = (selectedCard, selectedLocation);
            }

            // Resolve all player actions simultaneously
            foreach (var pair in playerActions)
            {
                Player player = pair.Key;
                (ICard card, Location location) = pair.Value;

                // Put the card on the selected location
                locationService.putCardToLocation(player, location, card, game);
            }

            // Update game state after all actions are resolved
            // UpdateGameState(game);
        }

        // Example methods for prompting player actions (selecting cards and locations)
        private ICard PromptPlayerSelectCard(Player player)
        {
            // Example: Prompt the player to select a card from their hand
            return player.selectedCard;
        }

        private Location PromptPlayerSelectLocation(Player player, ILocation[] locations)
        {
            // Example: Prompt the player to select a location from available locations
            return player.selectedLocation;
        }

        // Example method for updating game state after all actions are resolved
        // private void UpdateGameState(Game game)
        // {
        //     // Example: Update game state after card placements
        //     game.UpdateStateAfterCardPlacement();
        // }

    }
}