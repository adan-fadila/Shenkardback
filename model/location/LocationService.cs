using System;
using System.Collections.Generic;
using Card_package;
using Game_package;
using Player_package;

namespace Location_package
{
    public class LocationService
    {
        private static LocationService? instance;
        private locationDataAccess locationDataAccess = locationDataAccess.getInstance();
        private LocationFactory locationFactory = new LocationFactory();
        private LocationService()
        {

        }
        public static LocationService getInstance()
        {
            if (instance == null)
            {
                instance = new LocationService();
            }
            return instance;
        }
        /*get {num} locations*/
        public Location[] getLocations(int num)
        {
            int numOfAllLocations = locationDataAccess.numOfLocations();
            Location[] locations = new Location[num];
            Random rnd = new Random();

            HashSet<int> usedIds = new HashSet<int>();

            for (int i = 0; i < num; i++)
            {
                int id;
                do
                {
                    id = rnd.Next(1, numOfAllLocations + 1);
                } while (usedIds.Contains(id));

                usedIds.Add(id);
                locations[i] = locationFactory.generate(id);

            }

            return locations;


        }
        /*put card on location and update location points*/
        public Game putCardToLocation(Player player, ILocation location1, ICard card, Game game)
        {

            foreach (ILocation location in game.locations)
            {
                if (location.id == location1.id)
                {
                    location.playCard(player, card);

                }
            }
            return game;
        }
        public Game applayEffect(ILocation location1, Game game){
              foreach (ILocation location in game.locations)
            {
                if (location.id == location1.id)
                {
                    location.applyEffect(game);

                }
            }
            return game;
        }

        /*reveal the Location*/
        public void revealLocation(Location location)
        {
            location.reveal();
        }
    }
}