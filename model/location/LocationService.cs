using System;
using System.Collections.Generic;
using Card_package;
using Game_package;
using Player_package;

namespace Location_package
{
    public class LocationService
    {
        private static LocationService instance;
        private locationDataAccess locationDataAccess = locationDataAccess.getInstance();
        private LocationFactory locationFactory = new LocationFactory();
        private LocationService()
        {

        }
        public static LocationService getInstance()
        {
            if (instance == null)
            {
                return new LocationService();
            }
            return instance;
        }
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
        public void putCardToLocation(Player player, Location location, ICard card,Game game){
            location.playCard(player,card);
            location.applyEffect(game);
        }
        public void revealLocation(Location location){
            location.reveal();
        }
    }
}