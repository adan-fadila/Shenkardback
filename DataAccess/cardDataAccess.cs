
using System.Collections.Generic;
using System.Data;
using Card_package;
using Microsoft.Data.Sqlite;

public class cardDataAccess
{

    private static cardDataAccess instance;
    private Database database = Database.getInstance();
    private cardDataAccess()
    {

    }
    public static cardDataAccess getInstance()
    {
        if (instance == null)
        {
            instance = new cardDataAccess();
        }
        return instance;
    }
    public string getCardName(int id)
    {
        string query = "select name from cards where id = " + id + ";";
        string name = "";
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        if (reader.Read())
        {
            name = reader.GetString(0);
        }
        database.closeConnection(reader);
        return name;
    }
    public string getCardDesc(int id)
    {
        string query = "select desc from cards where id = " + id + ";";
        string desc = "";
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        if (reader.Read())
        {
            desc = reader.GetString(0);
        }
        database.closeConnection(reader);

        return desc;
    }
    public int getCardCost(int id)
    {
        string query = "select cost from cards where id = " + id + ";";
        int cost = -1;
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        if (reader.Read())
        {
            cost = reader.GetInt16(0);
        }
        database.closeConnection(reader);

        return cost;
    }
    public int getCardPower(int id)
    {
        string query = "select power from cards where id = " + id + ";";
        int power = -1;
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        if (reader.Read())
        {
            power = reader.GetInt16(0);
        }
        database.closeConnection(reader);

        return power;
    }
    public string getCardType(int id)
    {
        string query = "select type from cards where id = " + id + ";";
        string type = "";
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        if (reader.Read())
        {
            type = reader.GetString(0);
        }
        database.closeConnection(reader);

        return type;
    }
    public string getCardAbility(int id)
    {
        string type = getCardType(id);

        string query = "select ability from " + type + "Cards where cardId = " + id + ";";
        string ability = "";
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        if (reader.Read())
        {
            ability = reader.GetString(0);
        }
        database.closeConnection(reader);
        return ability;
    }
    public void addRegularCard(string name, string desc, int cost, int power) {
        string query = $"INSERT INTO cards(name,desc,cost,power,type) VALUES ({name},{desc},{cost},{power},regular);";
        SqliteDataReader dataReader = database.openConnectionAndRunQuery(query);
     }
    public void deleteCard(int id) { }
    public List<int> getAllCards()
    {
        int i = 0;
        string query = "SELECT id FROM cards;";
        List<int> cardsId = new List<int>();
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        while (reader.Read())
        {
            cardsId.Add(reader.GetInt16(0));
            i++;
        }

        database.closeConnection(reader);


        return cardsId;
    }
    public List<int> getAllMasterCards()
    {
        int i = 0;
        string query = "SELECT CardID FROM masterCards;";
        List<int> cardsId = new List<int>();
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        while (reader.Read())
        {
            cardsId.Add(reader.GetInt16(0));
            i++;
        }
        database.closeConnection(reader);

        return cardsId;
    }
    public List<int> getAllAbilityCards()
    {
        int i = 0;
        string query = "SELECT cardID FROM abilityCards;";
        List<int> cardsId = new List<int>();
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        while (reader.Read())
        {
            cardsId.Add(reader.GetInt16(0));
            i++;
        }

        database.closeConnection(reader);


        return cardsId;
    }
    public List<int> getAllRegularCards()
    {
        int i = 0;
        string query = "SELECT id FROM cards where type = 'regular';";
        List<int> cardsId = new List<int>();
        SqliteDataReader reader = database.openConnectionAndRunQuery(query);
        while (reader.Read())
        {
            cardsId.Add(reader.GetInt16(0));
            i++;
        }

        database.closeConnection(reader);

        return cardsId;
    }
    public int[] getRandomCards(int regular, int ability, int master)
    {
        int[] defaultDeck = new int[regular + ability + master];
        List<int> masterCards = getAllMasterCards();
        List<int> abilityCards = getAllAbilityCards();
        List<int> regularCards = getAllRegularCards();
        int j = 0;
        for (int i = 0; i < master; i++)
        {
            defaultDeck[j] = masterCards[i];
            j++;
        }
        for (int i = 0; i < ability; i++)
        {
            defaultDeck[j] = abilityCards[i];
            j++;
        }
        for (int i = 0; i < regular; i++)
        {
            defaultDeck[j] = regularCards[i];
            j++;
        }
        return defaultDeck;

    }
    public void updateCard(int id, int cost, int power)
    {
        string query = $"UPDATE cards SET cost = {cost}, power = {power} WHERE id = {id};";
        /****************************************/
        /*TODO add code*/
    }
}
