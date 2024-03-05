using Card_package;
using Player_package;

class PlayerController
{
    private PlayerService playerService = PlayerService.getInstance();
    public int getPlayerEnergy(Player player)
    {
        return playerService.getPlayerEnergy(player);
    }
    public void updatePlayerEnergy(Player player, int en)
    {
        playerService.updatePlayerEnergy(player, en);
    }
    public ICard[] drawCard(Player player, int num)
    {

       return playerService.drawCard(player, num);
    }
    public ICard[] getPlayerCards(Player player)
    {
        return playerService.getPlayerCards(player);
    }
    public int validatePlayer(string username, string password){
        return playerService.validatePlayer(username,password);
    }
     public int validateAdmin(string username, string password){
        return playerService.validateAdmin(username,password);
    }
    public Player GetPlayer(string name){
        return playerService.GetPlayer(name);
    }
      public Player GetPlayer(int id){
        return playerService.GetPlayer(id);
    }
    public bool AddPlayer(string name, string pass){
       return playerService.AddPlayer(name,pass);
    }
}