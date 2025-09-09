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

    public void LisaaLemmikki()
    {
        Console.Write("Anna omistajan nimi: ");
        string? omistajanNimi = Console.ReadLine();

        Console.Write("Anna lemmikin nimi: ");
        string? lemmikinNimi = Console.ReadLine();

        Console.Write("Anna Lemmikin laji: ");
        string? laji = Console.ReadLine();

        // Luodaan yhteys tietokantaan.
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        // Haetaan tietokannasta omistajan id nimen perusteella.
        var command1 = connection.CreateCommand();
        command1.CommandText = "SELECT id FROM Omistajat WHERE nimi = @Nimi";
        command1.Parameters.AddWithValue("Nimi", omistajanNimi);
        var reader = command1.ExecuteReader();
        reader.Read();
        int omistaja_id = reader.GetInt32(0);
        reader.Close();

        // Lisätään lemmikki tietokantaan.
        var command2 = connection.CreateCommand();
        command2.CommandText = "INSERT INTO Lemmikit (omistaja_id, nimi, laji) VALUES (@Id, @Nimi, @Laji)";
        command2.Parameters.AddWithValue("Id", omistaja_id);
        command2.Parameters.AddWithValue("Nimi", lemmikinNimi);
        command2.Parameters.AddWithValue("Laji", laji);
        command2.ExecuteNonQuery();

        // Suljetaan yhteys.
        connection.Close();

        Console.Clear();
        Console.WriteLine("Lemmikki lisätty!");
        Thread.Sleep(1000);
        Console.Clear();
    }

    public void PaivitaPuhelinnumero()
    {
        Console.Write("Anna nimi, jolle puhelinnumero päivitetään: ");
        string? nimi = Console.ReadLine();

        Console.Write("Anna päivitettävä puhelinnumero: ");
        string? puh = Console.ReadLine();

        // Luodaan yhteys tietokantaan.
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        // Lisätään omistaja tietokantaan.
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Omistajat SET puhelinnumero = @Puh WHERE nimi = @Nimi";
        command.Parameters.AddWithValue("Puh", puh);
        command.Parameters.AddWithValue("Nimi", nimi);
        command.ExecuteNonQuery();

        // Suljetaan yhteys.
        connection.Close();

        Console.Clear();
        Console.WriteLine("Puhelinnumero päivitetty!");
        Thread.Sleep(1000);
        Console.Clear();
    }

    public string Tulosta()
    {
        // Luodaan yhteys tietokantaan.
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        // Haetaan tietokannasta omistajan id nimen perusteella.
        var command1 = connection.CreateCommand();
        command1.CommandText = "SELECT * FROM Omistajat";
        var reader1 = command1.ExecuteReader();
        string omistajat = "";
        while (reader1.Read())
        {
            omistajat += $"{reader1.GetInt32(0)} | {reader1.GetString(1)} | {reader1.GetString(2)}";
        }

        reader1.Close();

        var command2 = connection.CreateCommand();
        command2.CommandText = "SELECT * FROM Lemmikit";
        var reader2 = command2.ExecuteReader();
        string lemmikit = "";
        while (reader2.Read())
        {
            lemmikit += $"{reader2.GetInt32(0)} | {reader2.GetInt32(1)} | {reader2.GetString(2)} | {reader2.GetString(3)}";
        }

        reader2.Close();
        connection.Close();

        return omistajat + "\n" + lemmikit;
    }
}