using System.Data;
using Microsoft.Data.Sqlite;

public class playerDataAccess
{

    private static playerDataAccess instance;
    private Database database = Database.getInstance();
    private playerDataAccess()
    {

    }
    public static playerDataAccess getInstance()
    {
        if (instance == null)
        {
            instance = new playerDataAccess();
        }
        return instance;
    }
    public string getPlayerName(int id)
    {
        string query = "SELECT name FROM players where id = " + id + ";";
        string name = "";
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        if (reader.Read())
        {
            name = reader.GetString(0);
        }
        database.closeConnection(reader);
        return name;

    }
    public string getPlayerPass(int id)
    {
        string query = "SELECT password FROM players where id = " + id + ";";
        string name = "";
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        if (reader.Read())
        {
            name = reader.GetString(0);
        }
        database.closeConnection(reader);
        return name;

    }
    public string getPlayerPass(string name)
    {
        string query = $"SELECT password FROM players where name = '{name}';";
        string password = null;
        IDataReader reader = database.openConnectionAndRunQuery(query);
        if (reader.Read())
        {
            password = reader.GetString(0);
        }
        return password;

    }
    public int getPlayerId(string name)
    {
        string query =$"SELECT id FROM players where name = '{name}';";
        int id = -1;
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        if (reader.Read())
        {
            id = reader.GetInt16(0);
        }
        database.closeConnection(reader);
        return id;

    }
    public bool hasDeck(int id)
    {
        string query = "SELECT hasDeck FROM players where id = " + id + ";";
        int has = 0;
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        if (reader.Read())
        {
            has = reader.GetInt16(0);
        }
        database.closeConnection(reader);
        return (has == 1) ? true : false;

    }

}