using ProjektGenspil;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace Exercise_5
{
    public static class ConsoleUI
    {
        private static readonly GameRepository gameRepository = new GameRepository();
        private static readonly GameSorting gameSorting = new GameSorting();
        private static readonly GameSearch gameSearch = new GameSearch(gameRepository);

        // Method to set the size of the console window at launch automatically (full view).
        public static void ConsoleWindowSetup()
        {
            Console.Title = "Genspil Inventory System";
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
        }

        // Menu for Create, Update, Delete (Read is done with ShowGames()).
        public static void AddEditRemoveMenu()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Submenu: \n");

                Console.WriteLine("1) Add a new game");
                Console.WriteLine("2) Update a game");
                Console.WriteLine("3) Remove a game");
                Console.WriteLine("4) Exit to main menu.");

                Console.Write("\nPlease select an option: (#) ");
                if (!int.TryParse(Console.ReadLine(), out int crudMenuChoice))
                {
                    Console.WriteLine("Invalid option. Please enter a number.");
                    continue;
                }

                switch (crudMenuChoice)
                {
                    case 1:
                        Console.Clear();
                        gameRepository.AddGames();
                        Console.Clear();
                        continue;
                    case 2:
                        Console.Clear();
                        gameRepository.UpdateGame();
                        Console.Clear();
                        continue;
                    case 3:
                        Console.Clear();
                        gameRepository.DeleteGame();
                        Console.Clear();
                        continue;
                    case 4:
                        Console.Clear();
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Please select a valid option");
                        continue;
                }
            }
        }

        // Method for summarizing a game. It's used by AddGame() and UpdateField().
        public static void GameSummary(Game game)
        {
            Console.Clear();

            if (game == null)
            {
                return;
            }

            Console.WriteLine($"Summary of game (ID: {game.Id}): \n");

            Console.WriteLine($"1) Title: {game.Title}");
            Console.WriteLine($"2) Year: {game.Year}");
            Console.WriteLine($"3) Genre: {game.Genre}");
            Console.WriteLine($"4) Players: {game.Players}");
            Console.WriteLine($"5) Condition: {game.Condition}");
            Console.WriteLine($"6) Price: {game.Price}");
            Console.WriteLine($"7) Stock: {(game.Stock ? "\u001b[32mYes\u001b[0m" : "\u001b[31mNo\u001b[0m")}");
            Console.WriteLine($"8) Requested: {(game.Requested ? "Yes" : "No")}");
            if (game.Requested)
            {
                Console.WriteLine($"9) Requested by: {game.RequestedBy}");
            }
        }

        // Method to Read (Shows information for each game in gamesToDisplay). It's used both to show all games and in the SearchGames() method.
        public static void ShowGames(List<Game> gamesToDisplay)
        {
            if (gamesToDisplay == null)
            {
                return;
            }

            if (gamesToDisplay.Count == 0)
            {
                Console.WriteLine("The last was not empty in ShowGames... but here it prints anyway.");
                Console.ReadLine();
            }

            // Print names for columns with width formatting.
            Console.WriteLine($"{"Id",-8} {"Title",-50} {"Year",-8} {"Genre",-15} {"Players",-8} {"Condition",-8} {"Price",-8} {"Stock",-10} {"Requested",-10} {"Requested By",-10}");

            // Print game data in columns with width formatting.
            foreach (Game game in gamesToDisplay)
            {
                Console.WriteLine($"{game.Id,-8} {game.Title,-50} {game.Year,-7} {game.Genre,-15} {game.Players,-9} {game.Condition,-9} {game.Price,-8} {(game.Stock ? "\u001b[32mYes\u001b[0m" : "\u001b[31mNo\u001b[0m"),-19} {(game.Requested ? "Yes" : "No"),-10} {(game.Requested ? game.RequestedBy : ""),-10}");
            }

            Console.WriteLine("\nGames found: {0}\n", gamesToDisplay.Count);
            Console.Write("Press <enter> to continue.");
            Console.ReadLine();
        }

        public static void PrintStock(List<Game> gamesToSort)
        {
            Console.Clear();
            DateTime currentTime = DateTime.Now;
            List<Game> sortedGames = null;
            string sortedBy = "";

            Console.WriteLine("Options to sort by: \n");
            Console.WriteLine("1) By title");
            Console.WriteLine("2) By genre");
            Console.Write("\nHow do you want to sort the inventory? (#) ");
            if (!int.TryParse(Console.ReadLine(), out int sortby))
            {
                Console.WriteLine("Invalid option. Please enter a number.");
                PrintStock(gamesToSort);
                return;
            }

            switch (sortby)
            {
                case 1:
                    sortedBy = "Title";
                    sortedGames = gameSorting.QuickSort(gamesToSort, 0, gamesToSort.Count - 1, "title");
                    break;
                case 2:
                    sortedBy = "Genre";
                    sortedGames = gameSorting.QuickSort(gamesToSort, 0, gamesToSort.Count - 1, "genre");
                    break;
                default:
                    Console.WriteLine("Please select a valid option. Press <enter> to try again.");
                    Console.ReadLine();
                    PrintStock(gameRepository.gamesList);
                    return;
            }

            sortedGames?.RemoveAll(game => !game.Stock);

            Console.Clear();
            Console.WriteLine($"Sorted by: {sortedBy}");
            Console.WriteLine("Date: " + currentTime + "\n");
            ShowGames(sortedGames);
        }
    }
}
