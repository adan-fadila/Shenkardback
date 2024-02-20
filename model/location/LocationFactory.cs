using Game_package;

namespace Location_package
{
    public class LocationFactory : ILocationFactory
    {
        private locationDataAccess locationData= locationDataAccess.getInstance();
        public Location generate(int id)
        {
            string name = locationData.getLocationName(id);
            string desc = locationData.getLocationDesc(id);
            string locationBattleStrategyName = locationData.getLocationAbility(id);
            ILocationBattleStrategy locationBattleStrategy = null;
            if(locationBattleStrategyName == "NegativeZoneStrategy"){
               locationBattleStrategy= new LocationNegativeZoneStrategy() ;
            }
            if(locationBattleStrategyName == "ReplaceCardsStrategy"){
               locationBattleStrategy= new LocationReplaceCardsStrategy() ;
            }
            return new Location(name,desc,locationBattleStrategy);
        }
    }
}