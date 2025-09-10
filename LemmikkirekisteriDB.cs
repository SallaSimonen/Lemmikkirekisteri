namespace Lemmikkirekisteri;

using System.ComponentModel;
using Microsoft.Data.Sqlite;

public class LemmikkirekisteriDB
{
    private string _connectionString = "Data Source = lemmikkirekisteri.db";

    public LemmikkirekisteriDB()
    {
        // Luodaan yhteys tietokantaan.
        using (var connection = new SqliteConnection(_connectionString))
        {
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
        }
    }

    public void LisaaOmistaja()
    {
        string? nimi;
        string? puh;

        while (true)
        {
            Console.Write("Anna Omistajan nimi: ");
            nimi = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nimi))
            {
                Console.WriteLine("\nNimi vaaditaan!");
                Thread.Sleep(1000);
                Console.Clear();
            }
            else
            {
                nimi = nimi[0].ToString().ToUpper() + nimi.Substring(1);
                Console.Clear();
                break;
            }
        }

        while (true)
        {
            Console.WriteLine("Anna Omistajan nimi: " + nimi);
            Console.Write("Anna Omistajan puhelinnumero: ");
            puh = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(puh))
            {
                Console.WriteLine("\nPuhelinnumero vaaditaan!");
                Thread.Sleep(1000);
                Console.Clear();
            }
            else break;
        }

        // Luodaan yhteys tietokantaan.
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            // Lisätään omistaja tietokantaan.
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Omistajat (nimi, puhelinnumero) VALUES (@Nimi, @Puh)";
            command.Parameters.AddWithValue("Nimi", nimi);
            command.Parameters.AddWithValue("Puh", puh);
            command.ExecuteNonQuery();
        }

        Console.Clear();
        Console.WriteLine("Omistaja lisätty!");
        Thread.Sleep(1000);
        Console.Clear();
    }

    public void LisaaLemmikki()
    {
        string? omistajanNimi;
        string? lemmikinNimi;
        string? laji;
        while (true)
        {
            Console.Write("Anna omistajan nimi: ");
            omistajanNimi = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(omistajanNimi))
            {
                Console.WriteLine("\nOmistajan nimi vaaditaan!");
                Thread.Sleep(1000);
                Console.Clear();
            }
            else
            {
                Console.Clear();
                break;
            }
        }

        while (true)
        {
            Console.WriteLine("Anna omistajan nimi: " + omistajanNimi);
            Console.Write("Anna lemmikin nimi: ");
            lemmikinNimi = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(lemmikinNimi))
            {
                Console.WriteLine("\nLemmikin nimi vaaditaan!");
                Thread.Sleep(1000);
                Console.Clear();
            }
            else
            {
                lemmikinNimi = lemmikinNimi[0].ToString().ToUpper() + lemmikinNimi.Substring(1);
                Console.Clear();
                break;
            }
        }

        while (true)
        {
            Console.WriteLine("Anna omistajan nimi: " + omistajanNimi);
            Console.WriteLine("Anna lemmikin nimi: " + lemmikinNimi);
            Console.Write("Anna lemmikin laji: ");
            laji = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(laji))
            {
                Console.WriteLine("\nLemmikin laji vaaditaan!");
                Thread.Sleep(1000);
                Console.Clear();
            }
            else
            {
                laji = laji[0].ToString().ToUpper() + laji.Substring(1);
                Console.Clear();
                break;
            }
        }

        // Luodaan yhteys tietokantaan.
        using (var connection = new SqliteConnection(_connectionString))
        {
            string lowerNimi = omistajanNimi.ToLower();

            connection.Open();

            // Haetaan tietokannasta omistajan id nimen perusteella.
            var command1 = connection.CreateCommand();
            command1.CommandText = "SELECT id FROM Omistajat WHERE LOWER(nimi) = @Nimi";
            command1.Parameters.AddWithValue("Nimi", lowerNimi);
            object? id = command1.ExecuteScalar();

            if (id == null)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Anna omistajan nimi: " + omistajanNimi);
                    Console.WriteLine("Anna lemmikin nimi: " + lemmikinNimi);
                    Console.WriteLine("Anna lemmikin laji: " + laji);
                    Console.WriteLine("\nAntamaasi omistajaa ei löytynyt.");
                    Console.WriteLine("Haluatko lisätä omistajan rekisteriin? (y/n)");
                    string? input = Console.ReadLine().ToLower();
                    if (input == "y")
                    {
                        Console.WriteLine("\nSiirrytään Omistajan lisäämiseen.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        LisaaOmistaja();
                        return;
                    }
                    else if (input == "n")
                    {
                        Console.WriteLine("\nPalataan valikkoon.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("\nVäärä syöte");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }


                }
            }
            
            // Lisätään lemmikki tietokantaan.
            var command2 = connection.CreateCommand();
            command2.CommandText = "INSERT INTO Lemmikit (omistaja_id, nimi, laji) VALUES (@Id, @Nimi, @Laji)";
            command2.Parameters.AddWithValue("Id", id);
            command2.Parameters.AddWithValue("Nimi", lemmikinNimi);
            command2.Parameters.AddWithValue("Laji", laji);
            command2.ExecuteNonQuery();
        }

        Console.Clear();
        Console.WriteLine("Lemmikki lisätty!");
        Thread.Sleep(1000);
        Console.Clear();
    }

    public void PaivitaPuhelinnumero()
    {
        string? nimi;
        string? puh;
        while (true)
        {
            Console.Write("Anna nimi, jolle puhelinnumero päivitetään: ");
            nimi = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nimi))
            {
                Console.WriteLine("\nNimi vaaditaan!");
                Thread.Sleep(1000);
                Console.Clear();
            }
            else
            {
                Console.Clear();
                break;
            }
        }

        while (true)
        {
            Console.WriteLine("Anna nimi, jolle puhelinnumero päivitetään: " + nimi);
            Console.Write("Anna päivitettävä puhelinnumero: ");
            puh = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(puh))
            {
                Console.WriteLine("\nPuhelinnumero vaaditaan!");
                Thread.Sleep(1000);
                Console.Clear();
            }
            else
            {
                Console.Clear();
                break;
            }
        }

        // Luodaan yhteys tietokantaan.
        using (var connection = new SqliteConnection(_connectionString))
        {
            string lowerNimi = nimi.ToLower();

            connection.Open();

            // Tarkistetaan onko tuote jo tietokannassa
            var command1 = connection.CreateCommand();
            command1.CommandText = "SELECT id FROM Omistajat WHERE LOWER(nimi) = @Nimi";
            command1.Parameters.AddWithValue("Nimi", lowerNimi);
            object? id = command1.ExecuteScalar();
            
            // Lisätään omistaja tietokantaan.
            var command2 = connection.CreateCommand();
            command2.CommandText = "UPDATE Omistajat SET puhelinnumero = @Puh WHERE LOWER(nimi) = @Nimi";
            command2.Parameters.AddWithValue("Puh", puh);
            command2.Parameters.AddWithValue("Nimi", lowerNimi);
            command2.ExecuteNonQuery();
        }

        Console.Clear();
        Console.WriteLine("Puhelinnumero päivitetty!");
        Thread.Sleep(1000);
        Console.Clear();
    }

    public void EtsiPuhelinnumero()
    {
        string? nimi;
        while (true)
        {
            Console.Write("Anna lemmikin nimi: ");
            nimi = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nimi))
            {
                Console.WriteLine("\nLemmikin nimi vaaditaan!");
                Thread.Sleep(1000);
                Console.Clear();
            }
            else break;
        }

        // Luodaan yhteys tietokantaan.
        using (var connection = new SqliteConnection(_connectionString))
        {
            string lowerNimi = nimi.ToLower();

            connection.Open();

            // Lisätään omistaja tietokantaan.
            var command1 = connection.CreateCommand();
            command1.CommandText = "SELECT puhelinnumero FROM Omistajat WHERE id = (SELECT omistaja_id FROM Lemmikit WHERE LOWER(nimi) = @Nimi)";
            command1.Parameters.AddWithValue("Nimi", lowerNimi);
            object? puh = command1.ExecuteScalar();

            nimi = nimi[0].ToString().ToUpper() + nimi.Substring(1);

            Console.Clear();
            Console.WriteLine($"Lemmikin {nimi} omistajan puhelinnumero on: {puh}");
            Console.WriteLine("\nPaina mitä tahansa näppäintä palataksesi valikkoon.");
            Console.ReadKey();
            Console.Clear();

        }
    }
}