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
            string Image = locationData.getLocationImage(id);
            ILocationBattleStrategy locationBattleStrategy = null;
            if(locationBattleStrategyName == "NegativeTotalStrategy"){
               locationBattleStrategy= new NegativeTotalStrategy() ;
            }
            if(locationBattleStrategyName == "ReplaceTotalStrategy"){
                locationBattleStrategy= new ReplaceTotalStrategy() ;
            }
            if(locationBattleStrategyName == "ReplaceCardStrategy"){
                locationBattleStrategy = new ReplaceCardStrategy();
            }
            return new Location(id,name,desc,Image,locationBattleStrategy);
        }
    }
}