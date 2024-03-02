using Microsoft.Data.Sqlite;
using System;
using System.Data;

public class Database
{
    private static Database instance;
    private string connString;
    private SqliteConnection dbconn;
    private SqliteCommand dbcmd;
    private SqliteDataReader reader;

    private Database()
    {
        // Set connection string to your SQLite database
        connString = "Data Source=DB/shenkard.sqlite;";
    }

    public static Database getInstance()
    {
        if (instance == null)
        {
            instance = new Database();
        }
        return instance;
    }

    public SqliteDataReader openConnectionAndRunQuery(string query)
    {
        try
        {
            // Create a new SQLite connection using the connection string
            dbconn = new SqliteConnection(connString);
            dbconn.Open();

            // Create a command object and set its command text to the query
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = query;

            // Execute the query and return the data reader
            reader = dbcmd.ExecuteReader();
            return reader;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error executing query: " + ex.Message);
            return null; // Or throw the exception if appropriate for your application flow
        }
    }
    public void openConnectionAndRunQueryWithoutRead(string query)
    {
        try
        {
            // Create a new SQLite connection using the connection string
            dbconn = new SqliteConnection(connString);
            dbconn.Open();

            // Create a command object and set its command text to the query
            dbcmd = dbconn.CreateCommand();
            dbcmd.CommandText = query;

            // Execute the query and return the data reader
            dbcmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error executing query: " + ex.Message);

        }
    }

    public void closeConnection(SqliteDataReader reader)
    {
        if (reader != null && !reader.IsClosed)
        {
            reader.Close();
            reader.Dispose();
        }
        if (dbcmd != null)
        {
            dbcmd.Dispose();
        }
        if (dbconn != null && dbconn.State != ConnectionState.Closed)
        {
            dbconn.Close();
            dbconn.Dispose();
        }
    }
     public void closeConnection()
    {
       
        if (dbcmd != null)
        {
            dbcmd.Dispose();
        }
        if (dbconn != null && dbconn.State != ConnectionState.Closed)
        {
            dbconn.Close();
            dbconn.Dispose();
        }
    }
}
