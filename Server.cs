using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Card_package;
using Game_package;
using Location_package;
using Player_package;
using SharedLibrary;


public class Server
{
    private TcpListener listener;
    Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();
    bool isRunning = false;
    int waitingPlayerId = -1;

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
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            // Split the message into parts using the '|' delimiter
            string[] parts = message.Split('|');

            // Determine the message type
            string messageType = parts[0];

            // Process the message based on its type
            switch (messageType)
            {
                case "LOGIN":
                    // Extract username and password from parts[1] and parts[2] respectively
                    string username = parts[1];
                    string password = parts[2];
                    HandleLogin(username, password, client);
                    // Process the login request
                    // Validate the username and password, send back player ID, etc.
                    break;
                case "GAME_REQUEST":
                    // Extract player ID from parts[1]
                    int playerId = int.Parse(parts[1]);
                    if (waitingPlayerId == -1)
                    {
                        waitingPlayerId = playerId;
                        
                    }
                    else
                    {
                        if (playerId != waitingPlayerId && waitingPlayerId != -1)
                        {
                            StartGame(waitingPlayerId, playerId, clients[waitingPlayerId], clients[playerId]);
                        }
                    }
                    // Process the game request
                    // Start a new game with the specified player ID, etc.
                    break;
                case "GAME_MOVE":
                    // Extract the move from parts[1]
                    // string move = parts[1];
                    // Process the game move
                    // Update the game state based on the move received, etc.
                    break;
                default:
                    // Handle unknown message types or errors
                    break;
            }
        }
    }

    public void StartGame(int player1, int player2, TcpClient client1, TcpClient client2)
    {
        // Create and initialize the game instance
        Game game = gameController.askForGame(player1, player2);
        // Serialize the game data
        GameData gameData = new GameData
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
            locationData.Player1Zone = GetPlayerCards(location.zone1.GetCards().ToList());
            locationData.Player2Zone = GetPlayerCards(location.zone2.GetCards().ToList());
            locationData.Player1LocatinScore = location.zone1.total;
            locationData.Player2LocatinScore = location.zone2.total;
        }
        return locationData;
    }
    // private AskForGameMessage ReadAskForGameMessage(NetworkStream stream)
    // {
    //     // Implement reading AskForGameMessage
    // }


    private void SendMessageToClient(TcpClient client, string message)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        stream.Write(buffer, 0, buffer.Length);
    }
    private void HandleLogin(string username, string password, TcpClient client)
    {

        NetworkStream stream = client.GetStream();

        // Implement login validation logic here
        int playerId = playerController.validatePlayer(username, password);
        if (playerId != -1)
        {
            clients.Add(playerId, client);
        }
        byte[] responseData = Encoding.UTF8.GetBytes(playerId.ToString());
        stream.Write(responseData, 0, responseData.Length);

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

    }
}

