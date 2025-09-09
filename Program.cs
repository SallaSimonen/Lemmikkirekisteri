namespace Lemmikkirekisteri;

class Program
{
    static void Main(string[] args)
    {
        LemmikkirekisteriDB rekisteri = new LemmikkirekisteriDB();

        Console.WriteLine("Tervetuloa Lemmikkirekisteriin!");
        Thread.Sleep(1000);
        Console.Clear();

        while (true)
        {
            Console.WriteLine("Lisää omistaja: 1\nLisää lemmikki: 2\nPäivitä puhelinnumero: 3\nEtsi puhelinnumero: 4\nLopeta:  0");
            string? vastaus = Console.ReadLine();

            switch (vastaus)
            {
                case "1":
                    Console.Clear();
                    rekisteri.LisaaOmistaja();
                    break;

                case "2":
                    Console.Clear();
                    rekisteri.LisaaLemmikki();
                    break;

                case "3":
                    Console.Clear();
                    rekisteri.PaivitaPuhelinnumero();
                    break;

                case "4":
                    Console.Clear();
                    rekisteri.EtsiPuhelinnumero();
                    break;

                case "0":
                    Console.Clear();
                    Console.WriteLine("Kiitos, kun käytit Lemmikkirekisteriä!");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return;
                    
                default:
                    Console.WriteLine("Anna 1, 2, 3, 4 tai 0");
                    break;
            }

        }
    }
}
