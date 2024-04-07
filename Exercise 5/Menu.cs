using System;
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


        public void SearchGames()
        {
            Console.WriteLine("Available search criteria:");
            Console.WriteLine("1) Name");
            Console.WriteLine("2) Price");
            Console.WriteLine("3) Condition (1-10)");
            Console.WriteLine("4) Players");
            Console.WriteLine("5) Stock\n");

            Console.Write("Enter search criteria (the number): ");
            string criteria = Console.ReadLine().ToLower();

            Console.Write("Enter search value: ");
            string searchValue = Console.ReadLine().ToLower();

            // A list to hold the game objects that meet the search criteria.
            List<Game> searchResults = new List<Game>();

            try
            {
                switch (criteria)
                {
                    case "1":
                        searchResults = gamesList.Where(game => game.Title.ToLower().Contains(searchValue)).ToList();
                        break;
                    case "2":
                        int price = int.Parse(searchValue);
                        searchResults = gamesList.Where(game => game.Price == price).ToList();
                        break;
                    case "3":
                        int condition = int.Parse(searchValue);
                        searchResults = gamesList.Where(game => game.Condition == condition).ToList();
                        break;
                    case "4":
                        searchResults = gamesList.Where(game => game.Players.ToLower().Contains(searchValue)).ToList();
                        break;
                    case "5":
                        bool stockValue = searchValue == "yes";
                        searchResults = gamesList.Where(game => game.Stock == stockValue).ToList();
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
                    Console.WriteLine("Search results:\n");
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




        // Method that loads the standard games library from a txt.file as setup.
        public void LoadGamesFromFile()
        {
            try
            {
                // Loads a pre-set file instead of a specified file.
                using (StreamReader sr = new StreamReader("C:\\Users\\mbaro\\Desktop\\GenspilGames.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] gameData = line.Split(','); // Assuming comma-separated values.
                        if (gameData.Length == 7) // Ensure all six fields are present in each line (i.e. the split gameData array has six indexes).
                        {
                            // Parse each piece of information from the file from each index of the gameData array.
                            string title = gameData[0];
                            int year = int.Parse(gameData[1]);
                            string genre = gameData[2];
                            string players = gameData[3];
                            int condition = int.Parse(gameData[4]);
                            int price = int.Parse(gameData[5]);
                            bool stock = bool.Parse(gameData[6]);

                            // Add the new Game instance to the gamesList.
                            gamesList.Add(new Game(title, year, genre, players, condition, price/*, stock*/));
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
                Console.WriteLine("Please select an option: \n\n1) Add a new game \n2) Update a game \n3) Remove a game \n4) Exit to main menu. ");
                int crudMenuChoice = int.Parse(Console.ReadLine());
                switch (crudMenuChoice)
                {
                    case 1:
                        Console.Clear();
                        this.AddGames();
                        continue;
                    case 2:
                        Console.Clear();
                        this.UpdateGame();
                        continue;
                    case 3:
                        Console.Clear();
                        this.DeleteGame();
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
            Console.WriteLine("Please enter the following information about the new game. \n");

            try
            {
                Console.Write("Title: ");
                string title = Console.ReadLine();
                Console.Write("Year: ");
                int year = int.Parse(Console.ReadLine());
                Console.Write("Genre: ");
                string genre = Console.ReadLine();
                Console.Write("Players (x-y): ");
                string players = Console.ReadLine();
                Console.Write("Condition (1-10): ");
                int condition = int.Parse(Console.ReadLine());
                Console.Write("Price: ");
                int price = int.Parse(Console.ReadLine());
                //Console.WriteLine("In stock? (Yes/No)");
                //bool stock = bool.Parse(Console.ReadLine());

                gamesList.Add(new Game(title, year, genre, players, condition, price/*, stock*/));
            }
            catch (Exception)
            {
                Console.WriteLine("You entered something invalid. Press <enter> to try again.");
                Console.ReadLine();
                Console.Clear();
                AddGames();
            }

            GameSummary(gamesList[gamesList.Count - 1].Id);
        }

        // Method for summarizing a newly created game. 
        // Make into a more generic method to print verifying information on a single game for both AddGame() UpdateGame() and DeleteGame() taking ID as parameter?
        // Ved AddGame() er spil ID'et gamesList.Count, ved UpdateGame(); sættes ID i metoden, og ved DeleteGame() er al information lige blevet slettet for ID'et...
        public void GameSummary(int id)
        {
            while (true)
            {
                Console.Clear();
                // Search for the game with the specified ID from parameter.
                Game game = gamesList.FirstOrDefault(g => g.Id == id);

                if (game != null)
                {
                    Console.WriteLine("\nSummary:\n");
                    Console.WriteLine($"Title: {game.Title}");
                    Console.WriteLine($"Year: {game.Year}");
                    Console.WriteLine($"Genre: {game.Genre}");
                    Console.WriteLine($"Players: {game.Players}");
                    Console.WriteLine($"Condition: {game.Condition}");
                    Console.WriteLine($"Price: {game.Price}\n");

                    Console.WriteLine("Press <enter> to return.");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }
            }
        }

        // Method to Read (Shows information for each game in gamesList).
        public void ShowGames(List<Game> gamesToDisplay)
        {
            int i = 1;
            Console.WriteLine("Games found: \n");
            foreach (Game game in gamesToDisplay)
            {
                Console.WriteLine($"Game Entry no.: {i}");
                Console.WriteLine($"Game Id: {game.Id}");
                Console.WriteLine($"Title: {game.Title}");
                Console.WriteLine($"Year: {game.Year}");
                Console.WriteLine($"Genre: {game.Genre}");
                Console.WriteLine($"Players (x-y): {game.Players}");
                Console.WriteLine($"Condition: {game.Condition}");
                Console.WriteLine($"Price (DKK): {game.Price}");
                Console.WriteLine("Stock: {0}", game.Stock == true ? "Yes" : "No");

                // Adds a blank line between each game.
                Console.WriteLine();
                i++;
            }
            Console.WriteLine("End of list. Press <enter> to return to menu.");
            Console.ReadLine();
            i = 1;
            return;
        }


        // Method to Update
        public void UpdateGame()
        {
            Console.Clear();

            Game game = GetGameById(gamesList);

            if (game != null)
            {
                Console.WriteLine($"You have selected {game.Title} to update. Is this correct? (Y/N)");
                verifyChoice = Console.ReadLine().ToLower();

                if (verifyChoice.ToLower() == "y")
                {
                    UpdateField(game);
                    return;
                }
                else
                {
                    UpdateGame();
                }
            }
            else
            {
                Console.WriteLine("You entered something invalid. Press <enter> to try again.");
                Console.ReadLine();
                UpdateGame();
            }

            AddEditRemoveMenu();
        }

        // Method for updating fields in UpdateGame() and VerifyNewGame();
        // Needs to reset variables: id, editNewGameField and valueNewGameField, after using them ?
        public void UpdateField(Game game)
        {
            Console.Clear();

            Console.WriteLine("Summary: \n");
            Console.WriteLine($"Title: {game.Title}");
            Console.WriteLine($"Year: {game.Year}");
            Console.WriteLine($"Genre: {game.Genre}");
            Console.WriteLine($"Players: {game.Players}");
            Console.WriteLine($"Condition: {game.Condition}");
            Console.WriteLine($"Price: {game.Price}");

            Console.Write("\nSelect a field to correct (write its name): ");
            // Need validation of user input here, valueNewGameField is validated in the switch
            var gameField = Console.ReadLine();
            Console.Write($"Please enter the updated value for {gameField}: ");
            var fieldValue = Console.ReadLine();

            try
            {
                switch (gameField.ToLower())
                {
                    case "title":
                        game.Title = fieldValue;
                        break;
                    case "year":
                        game.Year = int.Parse(fieldValue);
                        break;
                    case "genre":
                        game.Genre = fieldValue;
                        break;
                    case "players":
                        game.Players = fieldValue;
                        break;
                    case "condition":
                        game.Condition = int.Parse(fieldValue);
                        break;
                    case "price":
                        game.Price = int.Parse(fieldValue);
                        break;
                    default:
                        Console.WriteLine("Invalid field name. Press <enter> to try again.");
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

            Console.Clear();
            Console.WriteLine($"{gameField} was succesfully updated to \"{fieldValue}\". Press <enter> to return to the menu.");
            Console.ReadLine();
            Console.Clear();
            return;
        }

        // A method for retrieivng a game by its ID and returning that specific object.
        public Game GetGameById(List<Game> gamesList)
        {
            int id;
            Console.Write("Please write the ID of the game: ");
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID. Please enter a valid integer ID.");
                return null;
            }

            // Search for the game with the specified ID from parameter.
            Game game = gamesList.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                Console.WriteLine($"Game with ID {id} not found.");
            }

            return game;
        }

        // Method for Delete (Removes a game from the gamesList).
        // Add option to exit to menu if no game is to be deleted.
        // Deletion must also remove the game in the txt.file, not just the list. Maybe write entire gamesList back out to file after deletion? (overwriting it all).
        public void DeleteGame() 
        {
            try
            {
                Game game = GetGameById(gamesList);                

                if (game != null)
                {
                    Console.WriteLine($"You have selected {game.Title} to update. Is this correct? (Y/N)");
                    verifyChoice = Console.ReadLine().ToLower();

                    if (verifyChoice.ToLower() == "y")
                    {
                        Console.Clear();
                        Console.WriteLine($"You have succesfully deleted the game, {game.Title}! \n\nPress <enter> to return to menu...");
                        gamesList.RemoveAt(gamesList.IndexOf(game));
                        Console.ReadLine();
                        AddEditRemoveMenu();
                    }
                    else
                    {
                        Console.Clear();
                        DeleteGame();
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
