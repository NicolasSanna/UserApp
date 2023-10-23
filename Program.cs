// Chargement des classes du dossier classes
using App.src.Controller;

// Espace de nom pour l'application
namespace App
{
    // Classe App
    class Program
    {
        // La méthode Main de l'application principale sert de point d'entrée
        static void Main(string[] args)
        {
            string[] optionNames =
            {
                "Utilisateur",
                "Quitter le programme"
            };

            int currentOption = 1;
            int totalOptions = optionNames.Length;

            while (true)
            {
                Console.Title = "App Utilisateurs en C#";
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(new string('#', 23));
                Console.WriteLine("# PROGRAMME PRINCIPAL #");
                Console.WriteLine(new string('#', 23));
                Console.ForegroundColor = ConsoleColor.Gray;

                for (int i = 1; i <= totalOptions; i++)
                {
                    if (i == currentOption)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    string currentOptionName = optionNames[i - 1];

                    Console.WriteLine($"{i}. {currentOptionName}");

                    Console.ResetColor();
                }

                Console.WriteLine("Veuillez choisir une option :");

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        currentOption = Math.Max(1, currentOption - 1);
                        break;

                    case ConsoleKey.DownArrow:
                        currentOption = Math.Min(totalOptions, currentOption + 1);
                        break;

                    case ConsoleKey.Enter:

                        switch (currentOption)
                        {
                            case 1:
                                UserController.Start();
                                break;

                            case 2:
                                Console.WriteLine("Vous avez choisi de quitter le programme.");
                                Environment.Exit(0);
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}