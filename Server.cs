
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
            

                    break;
                default:
                    break;
            }
        }
    }

    private void StartGame(int player1, int player2, TcpClient client1, TcpClient client2)
    {
        Console.WriteLine("startGame");
        Game game = gameController.askForGame(player1, player2);
        games.Add(new List<int>() { player1, player2 }, game);
        GameData gameData = new GameData()
        {
            player1 = GetPlayerData(game.Players[0]),
            player2 = GetPlayerData(game.Players[1]),
            locationData1 = GetLocationData(game.locations[0], player1, player2),
            locationData2 = GetLocationData(game.locations[1], player1, player2),
            locationData3 = GetLocationData(game.locations[2], player1, player2),
        };
        string serializedGameData = JsonSerializer.Serialize(gameData);
        // Send the serialized game data to both players
        SendMessageToClient(client1, serializedGameData);
        SendMessageToClient(client2, serializedGameData);
    }
    private PlayerData GetPlayerData(Player player)
    {
        PlayerData player1 = new PlayerData
        {
            PlayeId = player.id,
            PlayeName = player.name,
            HandCards = GetPlayerCards(player.displayedCards),
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
                Power = card.Power
            };
            HandCards.Add(cardData);
        }
        return HandCards;
    }
    private LocationData GetLocationData(ILocation location, int player1, int player2)
    {
        LocationData locationData = new LocationData
        {
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

    private void EndTurn()
    {

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
        // GameController gameController = new GameController();
        // Game game = gameController.askForGame(1,2);
        // Console.WriteLine(game);
        // gameController.putCardToLocation(game.Players[1],(Location)game.locations[1],game.Players[1].displayedCards[0]);
        // Console.WriteLine("---------------------------------------------");
        // Console.WriteLine(game);
    }
}

