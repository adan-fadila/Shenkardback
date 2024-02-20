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

    private IBattleStrategy battleStrategy;
    public Game(Player player1, Player player2, IBattleStrategy battleStrategy, Location[] locations)
    {
      this.battleStrategy = battleStrategy;
      Players = new Player[2];
      Players[0] = player1;
      Players[1] = player2;
      this.locations = locations;
      this.turn = 0;
    }
    public bool endTurn()
    {
      if (this.turn < numOfTurns){

         this.turn++;
         return true;
      }
       return false;
    }

    public List<int> getWinner()
    {

      return this.battleStrategy.battle(this);
    }


  }
}
