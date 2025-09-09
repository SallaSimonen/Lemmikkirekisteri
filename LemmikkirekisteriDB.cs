namespace Lemmikkirekisteri;

using Microsoft.Data.Sqlite;

public class LemmikkirekisteriDB
{
    private string _connectionString = "Data Source = lemmikkirekisteri.db";

    public LemmikkirekisteriDB()
    {
        // Luodaan yhteys tietokantaan.
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        // Luodaan taulu Omistajat sarakkeet id, nimi, puhelinnumero.
        var command1 = connection.CreateCommand();
        command1.CommandText = @"CREATE TABLE IF NOT EXISTS Omistajat (
            id INTEGER PRIMARY KEY,
            nimi TEXT,
            puhelinnumero VARCHAR(20)
            )";

        command1.ExecuteNonQuery();

        // Luodaan taulu Lemmikit sarakkeet id, omistaja_id, nimi, laji.
        var command2 = connection.CreateCommand();
        command2.CommandText = @"CREATE TABLE IF NOT EXISTS Lemmikit (
            id INTEGER PRIMARY KEY,
            omistaja_id INTEGER,
            nimi TEXT, 
            laji TEXT,
            FOREIGN KEY (omistaja_id) REFERENCES Omistajat(id)
            )";

        command2.ExecuteNonQuery();

        // Suljetaan yhteys.
        connection.Close();
    }

    public void LisaaOmistaja()
    {
        Console.Write("Anna Omistajan nimi: ");
        string? nimi = Console.ReadLine();

        Console.Write("Anna Omistajan puhelinnumero: ");
        string? puh = Console.ReadLine();

        // Luodaan yhteys tietokantaan.
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        // Lisätään omistaja tietokantaan.
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Omistajat (nimi, puhelinnumero) VALUES (@Nimi, @Puh)";
        command.Parameters.AddWithValue("Nimi", nimi);
        command.Parameters.AddWithValue("Puh", puh);
        command.ExecuteNonQuery();

        // Suljetaan yhteys.
        connection.Close();

        Console.Clear();
        Console.WriteLine("Omistaja lisätty!");
        Thread.Sleep(1000);
        Console.Clear();
    }
}