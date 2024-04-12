using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ProjektGenspil
{
    internal class Menu
    {
        // String for verifying user choice in VerifyNewGame() and DeleteGame().
        string verifyChoice;
        // List that holds instances of Game.cs.
        public List<Game> gamesList = new List<Game>();

        public void PrintStock(List<Game> gamesToSort)
        {
            Console.Clear();
            DateTime currentTime = DateTime.Now;
            List<Game> sortedGames = null;
            string sortedBy = "";

            Console.WriteLine("Options to sort by: \n");
            Console.WriteLine("1) By title");
            Console.WriteLine("2) By genre");
            Console.Write("\nHow do you want to sort the inventory? (#) ");
            int.TryParse(Console.ReadLine(), out int sortby);

            switch (sortby)
            {                
                case 1:
                    sortedBy = "Title";
                    sortedGames = QuickSort(gamesToSort, 0, gamesToSort.Count - 1, "title");
                    break;
                case 2:
                    sortedBy = "Genre";
                    sortedGames = QuickSort(gamesToSort, 0, gamesToSort.Count - 1, "genre");
                    break;
                case 3:
                    Console.WriteLine("Please select a valid option. Press <enter> to try again.");
                    Console.ReadLine();
                    PrintStock(gamesList);
                    break;
            }

            sortedGames.RemoveAll(game => game.Stock == false);

            Console.Clear();
            Console.WriteLine($"Sorted by: {sortedBy}");
            Console.WriteLine("Date: " + currentTime + "\n");
            ShowGames(sortedGames);
            return;
        }

        public List<Game> QuickSort(List<Game> games, int left, int right, string sortBy)
        {            
            if (left < right && sortBy == "title")
            {
                int pivotIndex = QuickSortByTitle(games, left, right);
                QuickSort(games, left, pivotIndex - 1, "title");
                QuickSort(games, pivotIndex + 1, right, "title");                
            }
            else if (left < right && sortBy == "genre")
            {
                int pivotIndex = QuickSortByGenre(games, left, right);
                QuickSort(games, left, pivotIndex - 1, "genre");
                QuickSort(games, pivotIndex + 1, right, "genre");
            }
            return games;
        }

        // Refactor: can property to sort by be supplied by a parameter? If so, combine QuickSortByTitle() and QuickSortByGenre().
        private int QuickSortByTitle(List<Game> games, int left, int right)
        {
            string pivot = games[right].Title;
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (string.Compare(games[j].Title, pivot) <= 0)
                {
                    i++;
                    Game temp = games[i];
                    games[i] = games[j];
                    games[j] = temp;
                }
            }

            Game tempPivot = games[i + 1];
            games[i + 1] = games[right];
            games[right] = tempPivot;

            return i + 1;
        }

        private int QuickSortByGenre(List<Game> games, int left, int right)
        {
            string pivot = games[right].Genre;
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (string.Compare(games[j].Genre, pivot) <= 0)
                {
                    i++;
                    Game temp = games[i];
                    games[i] = games[j];
                    games[j] = temp;
                }
            }

            Game tempPivot = games[i + 1];
            games[i + 1] = games[right];
            games[right] = tempPivot;

            return i + 1;
        }

        // To search for multiple games, keep the list, and ask after switch if they want to search for additonal search criteria? Loop around so both search results are added to list?
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

        // Method to set the size of the console window at lunch automatically (full view).
        public void ConsoleWindowSetup()
        {
            Console.Title = "Genspil Inventory System";
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
        }

        // Method to load the games from a txt.file (GenspilGames.txt) into a list (gamesList) as setup.
        //public void LoadGamesFromFile()
        //{
        //    try
        //    {
        //        string directoryPath = @"C:\Users\mbaro\Desktop";
        //        string fileName = "GenspilGames.txt";
        //        string filePath = Path.Combine(directoryPath, fileName);

        //        // Loads a pre-set file with a set path.
        //        using (StreamReader sr = new StreamReader(filePath))
        //        {
        //            string line;
        //            while ((line = sr.ReadLine()) != null)
        //            {
        //                string[] gameData = line.Split(','); // Assuming comma-separated values.                              

        //                if (gameData.Length == 9) // Ensure all nine fields are present in each line (since the gameData array with the split data should have nine indexes).
        //                {
        //                    // Parse each piece of information from the file from each index of the gameData array.
        //                    string title = gameData[0];
        //                    int year = int.Parse(gameData[1]);
        //                    string genre = gameData[2];
        //                    string players = gameData[3];
        //                    int condition = int.Parse(gameData[4]);
        //                    int price = int.Parse(gameData[5]);
        //                    bool stock = bool.Parse(gameData[6]);
        //                    bool requested = bool.Parse(gameData[7]);
        //                    string requestedBy = gameData[8];

        //                    // Add the new Game instance to the gamesList.
        //                    gamesList.Add(new Game(title, year, genre, players, condition, price, stock, requested, requestedBy));
        //                }
        //                else
        //                {
        //                    Console.WriteLine("Invalid format for game data in the file.");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred while loading games from file: {ex.Message}");
        //        Console.ReadLine();
        //        return;
        //    }
        //}

        //Alternative method to load a public.txt file from Google Drive(but saving cannot be done using this method, needs API?).
        public async Task LoadGamesFromFile()
        {
        try
        {
            string fileUrl = "https://drive.usercontent.google.com/download?id=1S8L4-vwd3eV712DxFQpZ6qtJJsaTY1w2";

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
                        // Parse each piece of information from the file
                        string title = gameData[0];
                        int year = int.Parse(gameData[1]);
                        string genre = gameData[2];
                        string players = gameData[3];
                        int condition = int.Parse(gameData[4]);
                        int price = int.Parse(gameData[5]);
                        bool stock = bool.Parse(gameData[6]);
                        bool requested = bool.Parse(gameData[7]);
                        string requestedBy = gameData[8];

                        // Add the new Game instance to the gamesList
                        gamesList.Add(new Game(title, year, genre, players, condition, price, stock, requested, requestedBy));
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


    // Method to save games in gamesList to a file (GenspilGamesSave!) (different from GenspilGames.txt to avoid overwriting, but format should be consistent between both files).
    public void SaveToFileFromList()
        {
            try
            {
                string directoryPath = @"C:\Users\mbaro\Desktop";
                string fileName = "GenspilGamesSave.txt";
                string filePath = Path.Combine(directoryPath, fileName);

                using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    foreach (Game game in gamesList)
                    {
                        sw.WriteLine($"{game.Title},{game.Year},{game.Genre},{game.Players},{game.Condition},{game.Price},{game.Stock},{game.Requested},{game.RequestedBy}");
                    }

                    Console.WriteLine($"Changes successfully saved to: {fileName}.");
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

        // Menu for Create, Update, Delete (Read is done with ShowGames()).
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
                        this.AddGames();
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

        // Method to Create (Adds a new game instance to gamesList).
        // Tilføj evt. bekræftelse, om bruger vil tilføje nyt spil, ellers skal de igennem hele opsætningen, og endda slette det igen bagefter.
        // Tilføj getter, setter logik i Game.cs til at sikre korrekt input?
        public void AddGames()
        {
            string requestedBy = "";
            Console.WriteLine("Enter the following information: \n");

            try
            {
                Console.Write("Title: ");
                string title = Console.ReadLine();
                Console.Write("Year (yyyy): ");
                int.TryParse(Console.ReadLine(), out int year);
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

                gamesList.Add(new Game(title, year, genre, players, condition, price, stock, requested, requestedBy));
            }
            catch (Exception)
            {
                Console.Write("You entered something invalid. Press <enter> to try again.");
                Console.ReadLine();
                Console.Clear();
                AddGames();
            }

            GameSummary(gamesList[gamesList.Count - 1]);
            Console.Write("\nPress <enter> to continue.");
            Console.ReadLine();
            return;
        }

        // Method for summarizing a game. It's used by AddGame() and UpdateField().
        public void GameSummary(Game game)
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
            Console.WriteLine($"7) Stock: {0}", game.Stock == true ? "\u001b[32mYes\u001b[0m" : "\u001b[31mNo\u001b[0m");
            Console.WriteLine("8) Requested: {0}", game.Requested == true ? "Yes" : "No");
            if (game.Requested == true)
            {
                Console.WriteLine($"9) Requested by: {game.RequestedBy}");
            }

            return;
        }

        // Method to Read (Shows information for each game in gamesToDisplay). It's used both to show all games and in the SearchGames() method.
        public void ShowGames(List<Game> gamesToDisplay)
        {
            if (gamesToDisplay == null) 
            {
                return;
            }
            // ØVERSTE ER GAMMEL KODE: Printer vertikalt i stedet for horisontalt som koden forneden i metoden. SLET HVIS ALLE ENIGE OM HORISONTALT ER BEDRE.

            //Console.WriteLine("Results: \n");
            //foreach (Game game in gamesToDisplay)
            //{
            //    Console.WriteLine($"Game Id: {game.Id}");
            //    Console.WriteLine($"Title: {game.Title}");
            //    Console.WriteLine($"Year: {game.Year}");
            //    Console.WriteLine($"Genre: {game.Genre}");
            //    Console.WriteLine($"Players: {game.Players}");
            //    Console.WriteLine($"Condition: {game.Condition}");
            //    Console.WriteLine($"Price (DKK): {game.Price}");
            //    Console.WriteLine("Stock: {0}", game.Stock == true ? "\u001b[32mYes\u001b[0m" : "\u001b[31mNo\u001b[0m");
            //    Console.WriteLine("Requested: {0}", game.Requested == true ? "Yes" : "No");
            //    if (game.Requested == true)
            //    {
            //        Console.WriteLine($"Requested by: {game.RequestedBy}");
            //    }
            //    // Adds a blank line between each game.
            //    Console.WriteLine();
            //}
            //Console.WriteLine("Games found: {0} \n", gamesToDisplay.Count());
            //Console.Write("Press <enter> to continue.");
            //Console.ReadLine();
            //return;

            // Print names for columns with width formatting.
            Console.WriteLine($"{"Id",-8} " +
                              $"{"Title",-50} " +
                              $"{"Year",-8} " +
                              $"{"Genre",-15} " +
                              $"{"Players",-8} " +
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
                                  $"{game.Year,-7} " +
                                  $"{game.Genre,-15} " +
                                  $"{game.Players,-9} " +
                                  $"{game.Condition,-9} " +
                                  $"{game.Price,-8} " +
                                  $"{(game.Stock ? "\u001b[32mYes\u001b[0m" : "\u001b[31mNo\u001b[0m"),-19} " +
                                  $"{(game.Requested ? "Yes" : "No"),-10} " +
                                  $"{(game.Requested ? game.RequestedBy : ""),-10}");
            }

            Console.WriteLine("\nGames found: {0}\n", gamesToDisplay.Count);
            Console.Write("Press <enter> to continue.");
            Console.ReadLine();
            return;
        }

        // Method to Update a game object. Instead make into generic verify choice method for several methods to use?
        // Just integrate directly into UpdateField()?
        public void UpdateGame()
        {
            Console.Clear();

            Game game = GetGameById(gamesList);

            if (game != null)
            {
                Console.WriteLine($"You have selected {game.Title}. Is this correct? (Y/N)");
                verifyChoice = Console.ReadLine().ToLower();

                if (verifyChoice.ToLower() == "y")
                {
                    UpdateField(game);
                    return;
                }
                else if (verifyChoice.ToLower() == "n")
                    UpdateGame();
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

        // Method for updating fields in UpdateGame() and VerifyNewGame();
        public void UpdateField(Game game)
        {
            Console.Clear();
            string fieldValue = "";

            GameSummary(game);

            Console.Write("\nSelect a field to correct (#): ");
            int.TryParse(Console.ReadLine(), out int gameField);

            // If "Requested" is chosen, also ask who it was requested by. The "Requested"-bool is just flipped.
            if (gameField == 8)
            {
                Console.Write("Please enter who it was requested by: ");
                fieldValue = Console.ReadLine();
            }
            // If "In stock" is chosen, no updated value is necessary. The "Stock"-bool is just flipped. Otherwise, ask for updated value.
            else if (gameField != 7/* && gameField != 8*/)
            {
                Console.Write($"Please enter the updated value: ");
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
                        game.Year = int.Parse(fieldValue);
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
                    default:
                        Console.WriteLine("Invalid option chosen. Press <enter> to try again.");
                        Console.ReadLine();
                        UpdateField(game);
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("You entered something invalid. Press <enter> to try again.");
                Console.ReadLine();
                Console.Clear();
                UpdateField(game);
            }

            GameSummary(game);
            Console.Write($"\n{game.Title} was succesfully updated. \nPress <enter> to continue.\n");
            Console.ReadLine();
            return;
        }

        // A method for retrieivng a game by its ID and returning that specific game object.
        public Game GetGameById(List<Game> gamesList)
        {
            int id;
            Console.Write("Please write the ID of the game: ");
            int.TryParse(Console.ReadLine(), out id);

            // Search for the game with the parameter IDs.
            Game game = gamesList.FirstOrDefault(game => game.Id == id);

            if (game == null)
            {
                Console.WriteLine($"Game with ID {id} not found. Press <enter> to return to menu.");
                Console.ReadLine();
                return null;
            }

            return game;
        }

        // Method for Delete (Removes a game from the gamesList).
        // Deletion must also remove the game in the txt.file, not just the list? Maybe write entire gamesList back out to file after deletion? (overwriting it all) or just have separate save option in main menu.
        public void DeleteGame() 
        {
            try
            {
                Game game = GetGameById(gamesList);

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

    }
}
