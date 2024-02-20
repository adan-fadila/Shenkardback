namespace Location_package
{
    public interface ILocation
    {
        string Name { get; }
        string Desc {get;}
        Zone zone1{get;}
         Zone zone2{get;}
        ILocationBattleStrategy battleStrategy { get; }
    }
}