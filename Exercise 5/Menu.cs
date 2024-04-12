using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GenspilSystem
{
    internal class Menu
    {         
        // List that holds instances of Game.cs.
        public List<Game> gamesList = new List<Game>();                

// CONSOLE SETUP METHOD, AND SYSTEM METHODS FOR SAVING AND LOADING

        // Method to set the name and size (full screen) of the console window at lunch automatically.
        public void ConsoleWindowSetup()
        {
            Console.Title = "Genspil Inventory System";
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
        }

        //Method to load a public .txt file from Google Drive (for our ease of use, but saving cannot be done using this method, only locally).
        public async Task LoadGamesFromFile()
        {
            try
            {
                string fileUrl = "https://drive.usercontent.google.com/download?id=1BWZME-CcRPyuUGS73lK6ZyAvt-iZBoBJ";

                // Download the contents of the text file through HttpClient method
                using (HttpClient client = new HttpClient())
                {
                    // Download the content of the text file to a string 
                    string fileContents = await client.GetStringAsync(fileUrl);

                    // Split the content into lines
                    string[] lines = fileContents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in lines)
                    {
                        string[] gameData = line.Split(','); // Assuming comma-separated values.

                        if (gameData.Length == 9) // Ensure all nine fields are present in each line
                        {
                            string title = gameData[0];
                            string age = gameData[1];
                            string genre = gameData[2];
                            string players = gameData[3];
                            int condition = int.Parse(gameData[4]);
                            int price = int.Parse(gameData[5]);
                            bool stock = bool.Parse(gameData[6]);
                            bool requested = bool.Parse(gameData[7]);
                            string requestedBy = gameData[8];

                            // Add the new Game instance to the gamesList.
                            gamesList.Add(new Game(title, age, genre, players, condition, price, stock, requested, requestedBy));
                        }
                        else
                        {
                            Console.WriteLine("Invalid format for game data in the file.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading games from file: {ex.Message}");
            }
        }

        // Method to save the game objects in gamesList to a file (GenspilGamesSave!) on user desktop.
        public void SaveToFileFromList()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = "GenspilGamesSave.txt";
                string filePath = Path.Combine(desktopPath, fileName);

                using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    foreach (Game game in gamesList)
                    {
                        sw.WriteLine($"{game.Title},{game.Age},{game.Genre},{game.Players},{game.Condition},{game.Price},{game.Stock},{game.Requested},{game.RequestedBy}");
                    }

                    Console.WriteLine($"Changes successfully saved to: {filePath}.");
                    Console.WriteLine("Press <enter> to continue.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving games list to file: {ex.Message}");
                Console.ReadLine();
                return;
            }
        }

// METHOD FOR SHOWING SUBMENU TO ACCESS CRUD METHODS.

        // Menu for Create, Update, Delete (Reading is done with ShowGames()).
        public void AddEditRemoveMenu()
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
                int crudMenuChoice = int.Parse(Console.ReadLine());

                switch (crudMenuChoice)
                {
                    case 1:
                        Console.Clear();
                        this.AddGame();
                        Console.Clear();
                        continue;
                    case 2:
                        Console.Clear();
                        this.UpdateGame();
                        Console.Clear();
                        continue;
                    case 3:
                        Console.Clear();
                        this.DeleteGame();
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

// METHODS FOR CRUD:
// (C)reating game objects (AddGame())
// (R)eading game objects (GameSummary() and ShowGames())
// (U)pdating game objects (UpdateGame())
// (D)eleting game objects (DeleteGame())

        // Method for creating a game object (adds a new game object to gamesList).
        public void AddGame()
        {
            string requestedBy = "";
            Console.WriteLine("Enter the following information: \n");

            try
            {
                Console.Write("Title: ");
                string title = Console.ReadLine();
                Console.Write("Age (x-y): ");
                string age = Console.ReadLine();
                Console.Write("Genre: ");
                string genre = Console.ReadLine();
                Console.Write("Players (x-y): ");
                string players = Console.ReadLine();
                Console.Write("Condition (1-10): ");
                int.TryParse(Console.ReadLine(), out int condition);
                Console.Write("Price (DKK): ");
                int.TryParse(Console.ReadLine(), out int price);
                Console.Write("In stock (Y/N): ");
                bool stock = Console.ReadLine().ToLower() == "y" ? true : false;
                Console.Write("Requested (Y/N): ");
                bool requested = Console.ReadLine().ToLower() == "y" ? true : false;
                if (requested == true)
                {
                    Console.Write("Requested by: ");
                    requestedBy = Console.ReadLine();
                }

                gamesList.Add(new Game(title, age, genre, players, condition, price, stock, requested, requestedBy));
            }
            catch (Exception)
            {
                Console.Write("You entered something invalid. Press <enter> to try again.");
                Console.ReadLine();
                Console.Clear();
                AddGame();
            }

            GameSummary(gamesList[gamesList.Count - 1]);
            Console.Write("\nPress <enter> to continue.");
            Console.ReadLine();
            return;
        }

        // Method for reading a SINGLE game object's properties. It's used by AddGame() and UpdateGame().
        public void GameSummary(Game game)
        {   
            Console.Clear();

            if (game == null)
            {
                return;
            }

            Console.WriteLine($"Summary of game (ID: {game.Id}): \n");

            Console.WriteLine($"1) Title: {game.Title}");
            Console.WriteLine($"2) Age: {game.Age}");
            Console.WriteLine($"3) Genre: {game.Genre}");
            Console.WriteLine($"4) Players: {game.Players}");
            Console.WriteLine($"5) Condition: {game.Condition}");
            Console.WriteLine($"6) Price: {game.Price}");
            // Color coding red if not in stock, green if in stock.
            Console.WriteLine($"7) Stock: {0}", game.Stock == true ? "\u001b[32mYes\u001b[0m" : "\u001b[31mNo\u001b[0m");
            Console.WriteLine("8) Requested: {0}", game.Requested == true ? "Yes" : "No");
            if (game.Requested == true)
            {
                Console.WriteLine($"9) Requested by: {game.RequestedBy}");
            }

            return;
        }

        // Method for reading the properties of ALL game objects in gamesToDisplay (gamesList). It's used by ShowGames() and SearchGames().
        public void ShowGames(List<Game> gamesToDisplay)
        {
            if (gamesToDisplay == null) 
            {
                return;
            }

            // Print names for columns with width formatting.
            Console.WriteLine($"{"Id",-8} " +
                              $"{"Title",-50} " +
                              $"{"Age",-7} " +
                              $"{"Genre",-15} " +
                              $"{"Players",-9} " +
                              $"{"Condition",-8} " +
                              $"{"Price",-8} " +
                              $"{"Stock",-10} " +
                              $"{"Requested",-10} " +
                              $"{"Requested By",-10}");

            // Print game data in columns with width formatting.
            foreach (Game game in gamesToDisplay)
            {
                Console.WriteLine($"{game.Id,-8} " +
                                  $"{game.Title,-50} " +
                                  $"{game.Age,-7} " +
                                  $"{game.Genre,-15} " +
                                  $"{game.Players,-9} " +
                                  $"{game.Condition,-9} " +
                                  $"{game.Price,-8} " +
                                  // Color coding red if not in stock, green if in stock.
                                  $"{(game.Stock ? "\u001b[32mYes\u001b[0m" : "\u001b[31mNo\u001b[0m"),-19} " +
                                  $"{(game.Requested ? "Yes" : "No"),-10} " +
                                  $"{(game.Requested ? game.RequestedBy : ""),-10}");
            }

            Console.WriteLine("\nGames found: {0}\n", gamesToDisplay.Count);
            Console.Write("Press <enter> to continue.");
            Console.ReadLine();
            return;
        }

        // Method for updating the properties of a game object.
        public void UpdateGame()
        {
            Console.Clear();
            string verifyChoice;

            Game game = GetGameById(gamesList);

            if (game != null)
            {
                Console.Clear();
                Console.WriteLine($"You have selected {game.Title}. Is this correct? (Y/N)");
                verifyChoice = Console.ReadLine().ToLower();

                if (verifyChoice == "y")
                {
                    while (true)
                    {
                        GameSummary(game);

                        Console.Write("\nSelect a field to correct (#): ");
                        int.TryParse(Console.ReadLine(), out int gameField);

                        if (gameField < 1 || gameField > 9)
                        {
                            Console.WriteLine("Invalid option chosen. Press <enter> to try again.");
                            Console.ReadLine();
                            continue;
                        }

                        string fieldValue = "";
                        if (gameField == 8)
                        {
                            Console.Write("Please enter who it was requested by: ");
                            fieldValue = Console.ReadLine();
                        }
                        else if (gameField != 7)
                        {
                            Console.Write("Please enter the updated value: ");
                            fieldValue = Console.ReadLine();
                        }

                        try
                        {
                            switch (gameField)
                            {
                                case 1:
                                    game.Title = fieldValue;
                                    break;
                                case 2:
                                    game.Age = fieldValue;
                                    break;
                                case 3:
                                    game.Genre = fieldValue;
                                    break;
                                case 4:
                                    game.Players = fieldValue;
                                    break;
                                case 5:
                                    game.Condition = int.Parse(fieldValue);
                                    break;
                                case 6:
                                    game.Price = int.Parse(fieldValue);
                                    break;
                                case 7:
                                    game.Stock = !game.Stock;
                                    break;
                                case 8:
                                    game.Requested = !game.Requested;
                                    game.RequestedBy = fieldValue;
                                    break;
                                case 9:
                                    game.RequestedBy = fieldValue;
                                    break;
                            }
                            Console.Clear();
                            GameSummary(game);

                            Console.WriteLine($"\n{game.Title} was successfully updated. Press <enter> to continue.");
                            Console.ReadLine();
                            break;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("You entered something invalid. Press <enter> to try again.");
                            Console.ReadLine();
                        }
                    }
                }
                else if (verifyChoice == "n")
                {
                    UpdateGame();
                }
                else
                {
                    Console.WriteLine("You entered something invalid. Press <enter> to try again.");
                    Console.ReadLine();
                    UpdateGame();
                }
            }
            else
            {
                Console.Write("Press <enter> to try again.");
                Console.ReadLine();
                UpdateGame();
            }

            AddEditRemoveMenu();
        }        

        // Method for deleting game objects (removes a game object from the gamesList).
        public void DeleteGame() 
        {
            try
            {
                Game game = GetGameById(gamesList);
                string verifyChoice;

                GameSummary(game);

                if (game != null)
                {
                    Console.Write($"\nDo you want to remove this entry? (Y/N): ");
                    verifyChoice = Console.ReadLine().ToLower();

                    if (verifyChoice.ToLower() == "y")
                    {
                        Console.Clear();
                        Console.Write($"You have succesfully deleted the game, {game.Title}! \n\nPress <enter> to continue.");
                        gamesList.RemoveAt(gamesList.IndexOf(game));
                        Console.ReadLine();
                        AddEditRemoveMenu();
                    }
                    if (verifyChoice.ToLower() == "n")
                    {
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Invalid game ID. Press <enter> to return to menu.");
                        Console.ReadLine();
                        return;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("You entered something incorrect. Press <enter> to try again.");
                Console.ReadLine();
                Console.Clear();
                DeleteGame();
            }

        }

// SEARCH METHODS FOR GAME OBJECTS:
// By ID (GetGameById()
// By user input (SearchGames())

        // A method for retrieivng a game by its ID and returning that specific game object.
        public Game GetGameById(List<Game> gamesList)
        {
            int id;
            Console.Write("Please write the ID of the game: ");
            int.TryParse(Console.ReadLine(), out id);

            // Search gamesList for the game object with the id from user input and return it if found.
            Game game = gamesList.FirstOrDefault(game => game.Id == id);

            if (game == null)
            {
                Console.WriteLine($"Game with ID {id} not found. Press <enter> to return to menu.");
                Console.ReadLine();
                return null;
            }

            return game;
        }

        // Method to search gamesList based on user input.
        public void SearchGames()
        {
            Console.Clear();
            string searchValue = "";
            // A list to hold the game objects that meet the search criteria.
            List<Game> searchResults = new List<Game>();

            string[] criterias = { "title", "genre", "players", "condition", "price" };

            Console.WriteLine("Available search criteria:\n");
            Console.WriteLine("1) Title");
            Console.WriteLine("2) Genre");
            Console.WriteLine("3) Players");
            Console.WriteLine("4) Condition (1-10)");
            Console.WriteLine("5) Price");
            Console.WriteLine("6) In stock");
            Console.WriteLine("7) Requested\n");

            Console.Write("Enter search criteria (#): ");
            int.TryParse(Console.ReadLine(), out int criteria);

            if (criteria > 7)
            {
                Console.WriteLine("Please select a valid option. Press <enter> to try again.");
                Console.ReadLine();
                SearchGames();
            }
            // If "In stock" or "Requested" is chosen, no search value is necessary (user is likely searching for "true"). Otherwise, ask for search value.
            else if (criteria != 6 && criteria != 7)
            {
                Console.Write($"Enter search value for {criterias[criteria - 1]}: ");
                searchValue = Console.ReadLine().ToLower();
            }

            try
            {
                switch (criteria)
                {
                    case 1:
                        // Lambda expression to search through objects in gamesList, and look for one, where the Title property contains the searchValue. Return found as a list, since searchResults is a list.
                        searchResults = gamesList.Where(game => game.Title.ToLower().Contains(searchValue)).ToList();
                        break;
                    case 2:
                        searchResults = gamesList.Where(game => game.Genre.ToLower().Contains(searchValue)).ToList();
                        break;
                    case 3:
                        searchResults = gamesList.Where(game => game.Players.ToLower().Contains(searchValue)).ToList();
                        break;
                    case 4:
                        int condition = int.Parse(searchValue);
                        searchResults = gamesList.Where(game => game.Condition == condition).ToList();
                        break;
                    case 5:
                        int price = int.Parse(searchValue);
                        searchResults = gamesList.Where(game => game.Price == price).ToList();
                        break;
                    case 6:
                        searchResults = gamesList.Where(game => game.Stock == true).ToList();
                        break;
                    case 7:
                        searchResults = gamesList.Where(game => game.Requested == true).ToList();
                        break;
                    default:
                        Console.WriteLine("Invalid search criteria. Press <enter> to try again.");
                        Console.ReadLine();
                        SearchGames();
                        return;
                }

                if (searchResults.Count > 0)
                {
                    Console.Clear();
                    ShowGames(searchResults);
                    return;
                }
                else
                {
                    Console.WriteLine("No games found matching the search criteria. Press <enter> to try again.");
                    Console.ReadLine();
                    SearchGames();
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid value format. Press <enter> to try again.");
                Console.ReadLine();
                SearchGames();
            }

            return;
        }

    }
}
