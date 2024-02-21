using System.Collections.Generic;
using Card_package;
using Location_package;
using Player_package;
namespace Game_package
{
  public class Game
  {
    private const int numOfTurns = 6;
    public Player[] Players { get; private set; }

    public ILocation[] locations { get; private set; }
    private int turn;
    public int numOfPlayersHaveEndedTurn {get;set;}
    private IBattleStrategy battleStrategy;
    public Game(Player player1, Player player2, IBattleStrategy battleStrategy, Location[] locations)
    {
      this.battleStrategy = battleStrategy;
      Players = new Player[2];
      Players[0] = player1;
      Players[1] = player2;
      this.locations = locations;
      numOfPlayersHaveEndedTurn = 0;
      foreach (ILocation location in locations)
      {
        location.setPlayer(player1.id, player2.id);
      }
      this.turn = 0;
    }
    public bool endTurn()
    {
      if (this.turn < numOfTurns)
      {

        this.turn++;
        return true;
      }
      return false;
    }

    public List<int> getWinner()
    {

      return this.battleStrategy.battle(this);
    }
        public override string ToString()
        {
            return $"Game - Player1: {this.Players[0]} , - Player2: {this.Players[1]} \n- Locations: {this.locations[0]},{this.locations[1]},{this.locations[2]}";
        }


    }
}
