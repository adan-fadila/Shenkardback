
using System.Data;
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
    private CardController cardController = new CardController();
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
            byte[] buffer = new byte[5024];
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

                            Game gameToRemove = games[keyWithplayerId];
                            if (gameToRemove.Players[0].id == disconnectedPlayerId)
                            {

                                SendMessageToClient(clients[gameToRemove.Players[1].id], "PlayerExit");
                            }
                            else
                            {
                                SendMessageToClient(clients[gameToRemove.Players[0].id], "PlayerExit");
                            }


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
                case "SignIn":
                    string newUsername = parts[1];
                    string newPassword = parts[2];
                    HandleSignIn(newUsername, newPassword);
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


                    string serializedGameData = parts[1];
                    List<PlayedCard>? playedCards = JsonSerializer.Deserialize<List<PlayedCard>>(serializedGameData);
                    int Id = GetKeyByValue(client);
                    List<int>? playersIds = games.Keys.FirstOrDefault(key => key.Contains(Id));
                    Game game = games[playersIds];
                    EndTurn(game, playedCards, client);





                    break;
                case "ExitGame":
                    int disconnectedPlayerId = clients.FirstOrDefault(x => x.Value == client).Key;

                    var keyWithplayerId = games.Keys.FirstOrDefault(key => key.Contains(disconnectedPlayerId));
                    if (keyWithplayerId != null)
                    {

                        Game gameToRemove = games[keyWithplayerId];
                        if (gameToRemove.Players[0].id == disconnectedPlayerId)
                        {

                            SendMessageToClient(clients[gameToRemove.Players[1].id], "PlayerExit");
                        }
                        else
                        {
                            SendMessageToClient(clients[gameToRemove.Players[0].id], "PlayerExit");
                        }


                        games.Remove(keyWithplayerId);
                        Console.WriteLine(games.Count);
                    }
                    break;

                case "GetCards":
                    SendCards(client);
                    break;
                case "UpdateCard":
                    try
                    {
                        int id = int.Parse(parts[1]);
                        int cost = int.Parse(parts[2]);
                        int power = int.Parse(parts[3]);
                        UpdateCards(client, id, cost, power);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("UpdateCards faild Wrong Parametes, " + e);
                    }

                    break;

                default:
                    break;
            }
        }
    }


    private void SendCards(TcpClient client)
    {
        List<ICard> cards = cardController.getCards();
        List<CardData> cardDatas = new List<CardData>();
        foreach (ICard card in cards)
        {
            cardDatas.Add(GetCardData(card));
        }
        string serializedGameData = JsonSerializer.Serialize(cardDatas);

        SendMessageToClient(client, serializedGameData);
    }

    public void UpdateCards(TcpClient client, int id, int cost, int power)
    {
        cardController.updateCardCost(id, cost);
        cardController.updateCardPower(id, power);
        ICard card = cardController.getCard(id);
        CardData cardData = GetCardData(card);
        string msg = JsonSerializer.Serialize(cardData);
        SendMessageToClient(client, msg);

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
            CardData cardData = GetCardData(card);
            HandCards.Add(cardData);
        }
        return HandCards;
    }
    private CardData GetCardData(ICard card)
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
        return cardData;
    }
    private LocationData GetLocationData(ILocation location, int player1, int player2)
    {
        LocationData locationData = new LocationData
        {
            Id = location.id,
            Name = location.Name,
            Desc = location.Desc,
            Image = location.Image
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

    private void HandleSignIn(string username, string password)
    {

        playerController.AddPlayer(username, password);

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
        if (game == null)
        {
            Console.WriteLine("game is null");
        }
        if (playedCards == null)
        {
            Console.WriteLine("playedcards is null");
        }
        if (client == null)
        {
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
            game = gameController.putCardToLocation(player, card.locationData.Id, card.cardData.id, game);

        }
        if (game.numOfPlayersHaveEndedTurn % game.Players.Length == 0)
        {
            /**************************/
            /*check if game ended*/
            /**************************/
            List<int>? keyWithplayerId = games.Keys.FirstOrDefault(key => key.Contains(GetKeyByValue(client)));
            if (gameController.endTurn(game))
            {
                string gameData = setGameData(game, game.Players[0].id, game.Players[1].id);


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
               
            }
            else
            {
                List<int> winners = gameController.startBattle(game);
                string serializedWinners = JsonSerializer.Serialize(winners);
                string msg = $"gameEnd|{serializedWinners}";
                foreach (int id in keyWithplayerId)
                {

                    TcpClient client1 = clients[id];
                    SendMessageToClient(client1, msg);
                    Console.WriteLine(serializedWinners);

                }
                games.Remove(keyWithplayerId);

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

    }
}

