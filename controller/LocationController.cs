using Location_package;

public class LocationController
{
    private LocationService locationService = LocationService.getInstance();
    public Location[] getLocations(int num){
        return locationService.getLocations(num);
    }
   
}