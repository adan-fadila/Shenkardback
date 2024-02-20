public interface IMessage
{
    MessageType MessageType { get; }
}

public class LoginMessage : IMessage
{
    public MessageType MessageType => MessageType.Login;
    public string username { get; set; }
    public string password { get; set; }
}

public class AskForGameMessage : IMessage
{
    public MessageType MessageType => MessageType.AskForGame;
    // Add any additional properties specific to AskForGame message
}

public enum MessageType
{
    Login,
    AskForGame,
    // Add more message types as needed
}
