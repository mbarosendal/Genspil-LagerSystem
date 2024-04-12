using Exercise_5;
using ProjektGenspil;

namespace Exercise_5
{
    class Program
    {
        static void Main(string[] args)
        {
            GameRepository gameRepository = new GameRepository();
            gameRepository.LoadGamesFromFile();
            GameSearch gameSearch = new GameSearch(gameRepository);
            Game game = gameRepository.GetGameById();

            Console.Write("Welcome to Genspil menu system! Press <enter> to proceed to the menu. ");
            Console.ReadLine();

            // Keeps user in the main menu.
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Main Menu:\n");

                Console.WriteLine("1) Show games");
                Console.WriteLine("2) Search games");
                Console.WriteLine("3) Add, edit or remove games");
                Console.WriteLine("4) Show requests");
                Console.WriteLine("5) Print inventory");
                Console.WriteLine("6) Exit ");
                Console.WriteLine("7) Save changes");

                Console.Write("\nPlease select an option (#): ");
                int menuChoice = int.Parse(Console.ReadLine());

                switch (menuChoice)
                {
                    // Show the menu.
                    case 1:
                        Console.Clear();
                        ConsoleUI.ShowGames(gameRepository.gamesList);
                        continue;
                    case 2:
                        Console.Clear();
                        gameSearch.SearchGames();
                        continue;
                    case 3:
                        Console.Clear();
                        ConsoleUI.AddEditRemoveMenu();
                        continue;
                    case 4:
                        Console.Clear();
                        //newMenu.ShowRequests();
                        continue;
                    case 5:
                        Console.Clear();
                        ConsoleUI.PrintStock(gameRepository.gamesList);
                        continue;
                    case 6:
                        Console.Clear();
                        Console.WriteLine("Thank you for using the service. You may close the program now.");
                        return;
                    case 7:
                        Console.Clear();
                        gameRepository.SaveToFileFromList();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Please select a valid option. Press <enter> to try again.");
                        Console.ReadLine();
                        continue;
                }
            }
        }
    }
}
