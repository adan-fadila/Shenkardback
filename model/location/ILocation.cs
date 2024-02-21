namespace Location_package
{
    public interface ILocation
    {

        int id { get; set; }
        string Name { get; }
        string Desc { get; }
        Zone zone1 { get; }
        Zone zone2 { get; }
        ILocationBattleStrategy battleStrategy { get; }
        public void setPlayer(int id1,int id2);
    }
}