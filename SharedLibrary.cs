namespace SharedLibrary
{
    using System.Collections.Generic;

public class GameData
{
    public PlayerData player1{get;set;}
  public  PlayerData player2{get;set;}
   
    public LocationData locationData1{ get; set; }
     public LocationData locationData2{ get; set; }
      public LocationData locationData3{ get; set; }

}
public class CardData
{

    public int id { get; set; }
    public string Name
    {
        get;
        set;
    }
    public string Desc
    {
        get;
        set;
    }
    public int Cost
    {
        get;
        set;
    }
    public int Power
    {
        get;
        set;
    }
}
public class PlayerData
{
    public string PlayeName { get; set; }
    public int PlayeId { get; set; }
    public List<CardData> HandCards { get; set; }

}
public class LocationData
{
    public string Name { get; set; }
    public string Desc { get; set; }
    public List<CardData> Player1Zone { get; set; }
    public int Player1LocatinScore { get; set; }
    public List<CardData> Player2Zone { get; set; }
     public int Player2LocatinScore { get; set; }
}


}