
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Card_package;
using Game_package;
using Location_package;
using Player_package;
using SharedLibrary;


public class Server
{
    private TcpListener listener;
    Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();
    Dictionary<List<int>, Game> games = new Dictionary<List<int>, Game>();
    bool isRunning = false;
    int waitingPlayerId = -1;
    private readonly object lockObject = new object();


    private PlayerController playerController = new PlayerController();
    private GameController gameController = new GameController();
    public Server(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
    }

    public void Start()
    {
        listener.Start();
        Console.WriteLine("Server is running...");
        isRunning = true;

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            Task.Run(() => HandleClient(client));
        }
    }

    private void HandleClient(TcpClient client)
    {
        while (isRunning)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead == 0)
            {
                // If bytesRead is 0, it means the client has disconnected
                Console.WriteLine("Client disconnected.");
                // Remove the disconnected client from the dictionary
                int disconnectedPlayerId = clients.FirstOrDefault(x => x.Value == client).Key;
                if (disconnectedPlayerId != 0)
                {
                    clients.Remove(disconnectedPlayerId);
                    // Handle any other cleanup tasks related to the disconnected client
                    try
                    {
                        var keyWithplayerId = games.Keys.FirstOrDefault(key => key.Contains(disconnectedPlayerId));
                        if (keyWithplayerId != null)
                        {
                            // Get the corresponding Game object
                            //end the game to the other Player !!!!!!!!!!!!!!!!!!!!!! not fineshed
                            Game gameToRemove = games[keyWithplayerId];
                            if (gameToRemove.Players[0].id == disconnectedPlayerId)
                            {
                                NetworkStream otherPlayStream = clients[gameToRemove.Players[1].id].GetStream();
                            }
                            else
                            {
                                NetworkStream otherPlayStream = clients[gameToRemove.Players[0].id].GetStream();
                            }

                            // Now you can use the 'game' object as needed 

                            games.Remove(keyWithplayerId);
                        }
                       ;
                    }
                    catch (System.Exception)
                    {

                        Console.WriteLine("no game  for this client");
                    }
                }
                break; // Exit the loop
            }
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            // Split the message into parts using the '|' delimiter
            string[] parts = message.Split('|');

            // Determine the message type
            string messageType = parts[0];

            switch (messageType)
            {
                case "LOGIN":
                    string username = parts[1];
                    string password = parts[2];
                    HandleLogin(username, password, client);
                    break;
                case "AdminLOGIN":
                    username = parts[1];
                    password = parts[2];
                    HandleAdminLogin(username, password, client);
                    break;
                case "GAME_REQUEST":
                    int playerId = int.Parse(parts[1]);
                    if (waitingPlayerId == -1)
                    {
                        lock (lockObject) { waitingPlayerId = playerId; }
                        GameData? gameData = null;
                        string nullGame = JsonSerializer.Serialize(gameData);
                        SendMessageToClient(clients[playerId], nullGame);

                    }
                    else
                    {
                        if (playerId != waitingPlayerId && waitingPlayerId != -1)
                        {
                            int player1Id = waitingPlayerId;
                            int player2Id = playerId;
                            waitingPlayerId = -1; // Reset waiting player ID for the next game

                            // Ensure to pass the correct player IDs to StartGame
                            StartGame(player1Id, player2Id, clients[player1Id], clients[player2Id]);

                        }
                    }

                    break;
                case "EndTurn":

                    // try
                    // {
                        string serializedGameData = parts[1];
                        List<PlayedCard>? playedCards = JsonSerializer.Deserialize<List<PlayedCard>>(serializedGameData);
                        int Id = GetKeyByValue(client);
                        List<int>? playersIds = games.Keys.FirstOrDefault(key => key.Contains(Id));
                        Game game = games[playersIds];
                        EndTurn(game, playedCards, client);

                    // }
                    // catch (Exception e)
                    // {
                    //     Console.WriteLine("HandleClient,EndTurn: " + e.Message);
                    // }

                    break;
                default:
                    break;
            }
        }
    }

    private void StartGame(int player1, int player2, TcpClient client1, TcpClient client2)
    {
        Game game = gameController.askForGame(player1, player2);
        games.Add(new List<int>() { player1, player2 }, game);
        string serializedGameData = setGameData(game, player1, player2);
        // Send the serialized game data to both players
        SendMessageToClient(client1, serializedGameData);
        SendMessageToClient(client2, serializedGameData);
    }
    string setGameData(Game game, int player1, int player2)
    {

        GameData gameData = new GameData()
        {
            player1 = GetPlayerData(game.Players[0]),
            player2 = GetPlayerData(game.Players[1]),
            locationDatas = new List<LocationData>()

        };
        foreach (ILocation location in game.locations)
        {
            gameData.locationDatas.Add(GetLocationData(location, player1, player2));

        }
        string serializedGameData = JsonSerializer.Serialize(gameData);
        return serializedGameData;
    }
    private PlayerData GetPlayerData(Player player)
    {
        PlayerData player1 = new PlayerData
        {
            PlayeId = player.id,
            PlayeName = player.name,
            HandCards = GetPlayerCards(player.displayedCards),
            Energy = player.energy
        };
        return player1;
    }
    private List<CardData> GetPlayerCards(List<ICard> cards)
    {
        List<CardData> HandCards = new List<CardData>();
        if (cards == null || cards.Count == 0)
        {
            return HandCards;
        }
        foreach (ICard card in cards)
        {
            CardData cardData = new CardData
            {
                id = card.id,
                Name = card.Name,
                Desc = card.Desc,
                Cost = card.Cost,
                Power = card.Power,
                Image = card.Image

            };
            HandCards.Add(cardData);
        }
        return HandCards;
    }
    private LocationData GetLocationData(ILocation location, int player1, int player2)
    {
        LocationData locationData = new LocationData
        {
            Id = location.id,
            Name = location.Name,
            Desc = location.Desc,
        };
        if (location.zone1.Player == player1)
        {
            locationData.Player1Zone = GetPlayerCards([.. location.zone1.GetCards()]);

            locationData.Player2Zone = GetPlayerCards([.. location.zone2.GetCards()]);



            locationData.Player1LocatinScore = location.zone1.total;
            locationData.Player2LocatinScore = location.zone2.total;
        }
        return locationData;
    }



    private void SendMessageToClient(TcpClient client, string message)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        stream.Write(buffer, 0, buffer.Length);
    }
    
    private void HandleLogin(string username, string password, TcpClient client)
    {

        NetworkStream stream = client.GetStream();

        int playerId = playerController.validatePlayer(username, password);
        if (playerId != -1)
        {
            clients.Add(playerId, client);
        }
        byte[] responseData = Encoding.UTF8.GetBytes(playerId.ToString());
        stream.Write(responseData, 0, responseData.Length);

    }









    private void HandleAdminLogin(string username, string password, TcpClient client)
    {

        NetworkStream stream = client.GetStream();

        int AdminId = playerController.validateAdmin(username, password);
        if (AdminId != -1)
        {
            // clients.Add(playerId, client);
        }
        byte[] responseData = Encoding.UTF8.GetBytes(AdminId.ToString());
        stream.Write(responseData, 0, responseData.Length);

    }















    private void EndTurn(Game game, List<PlayedCard> playedCards, TcpClient client)
    {
        if(game == null){
            Console.WriteLine("game is null");
        }
        if(playedCards == null){
            Console.WriteLine("playedcards is null");
        }
        if(client == null){
            Console.WriteLine("client is null");
        }
        game.numOfPlayersHaveEndedTurn++;
        int playerId = GetKeyByValue(client);
        if (playerId == -1)
        {
            Console.WriteLine("client not founded");
        }
        Player player = getPlayerbyId(game, playerId);
        foreach (PlayedCard card in playedCards)
        {
            Console.WriteLine(card.cardData.Name +" to  "+ card.locationData.Name);
            game = gameController.putCardToLocation(player, card.locationData.Id, card.cardData.id, game);

        }
        if (game.numOfPlayersHaveEndedTurn == game.Players.Length)
        {
            Console.WriteLine("2 clicked");
            /**************************/
            /*check if game ended*/
            /**************************/
            if (gameController.endTurn(game))
            {
                string gameData = setGameData(game, game.Players[0].id, game.Players[1].id);
                Console.WriteLine(gameData);
                List<int>? keyWithplayerId = games.Keys.FirstOrDefault(key => key.Contains(GetKeyByValue(client)));
                try
                {
                    foreach (int id in keyWithplayerId)
                    {

                        TcpClient client1 = clients[id];
                        SendMessageToClient(client1, gameData);

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("End Turn Exception" + e);
                }
                /********************/
                /***need completeing***/
            }
            else
            {
                gameController.startBattle(game);
            }


        }


    }















    int GetKeyByValue(TcpClient client)
    {
        // Iterate through the dictionary
        foreach (var pair in clients)
        {
            // If the current value matches the desired value, return its key
            if (pair.Value.Equals(client))
            {
                return pair.Key;
            }
        }
        // If the value is not found, return a default value
        return -1; // Or throw an exception, depending on your requirements
    }





    private Player getPlayerbyId(Game game, int id)
    {
        if (game.Players[0].id == id)
        {
            return game.Players[0];
        }
        return game.Players[1];

    }


    public void Stop()
    {
        listener.Stop();
    }
}

public class ServerMain
{
    static void Main(string[] args)
    {

        Server server = new Server(8888);
        server.Start();
    //     GameController gameController = new GameController();
    //     Game game = gameController.askForGame(1,2);
    //     Console.WriteLine(game);
    //    game = gameController.putCardToLocation(game.Players[1],game.locations[1].id,game.Players[1].displayedCards[0].id,game);
     
    //     Console.WriteLine("---------------------------------------------");
    //     Console.WriteLine(game);
    }
}

