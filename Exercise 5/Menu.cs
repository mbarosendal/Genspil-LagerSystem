using System;
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
        // These hold information for a new game to be added.
        string Title;
        int Year;
        string Genre;
        string Players;
        int Condition;
        int Price;
        // Int for writing out the index of games in ShowGames().
        int i = 1;
        // String for verifying user choice in VerifyNewGame() and DeleteGame().
        string verifyChoice;
        // Int that points to the id of game for DeleteGame().
        int id = 0;   
        // List that holds instances of Game.cs.
        public List<Game> gamesList = new List<Game>();

        // The Constructor.
        //public Menu()
        //{
        //}

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
                        if (gameData.Length == 6) // Ensure all six fields are present in each line (i.e. the split gameData array has six indexes).
                        {
                            // Parse each piece of information from the file from each index of the gameData array.
                            string title = gameData[0];
                            int year = int.Parse(gameData[1]);
                            string genre = gameData[2];
                            string players = gameData[3];
                            int condition = int.Parse(gameData[4]);
                            int price = int.Parse(gameData[5]);

                            // Add the new Game instance to the gamesList.
                            gamesList.Add(new Game(title, year, genre, players, condition, price));
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
                Title = Console.ReadLine();
                Console.Write("Year: ");
                Year = int.Parse(Console.ReadLine());
                Console.Write("Genre: ");
                Genre = Console.ReadLine();
                Console.Write("Players (x-y): ");
                Players = Console.ReadLine();
                Console.Write("Condition (1-10): ");
                Condition = int.Parse(Console.ReadLine());
                Console.Write("Price: ");
                Price = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("You entered something invalid. Press <enter> to try again.");
                Console.ReadLine();
                Console.Clear();
                AddGames();
            }

            gamesList.Add(new Game(Title, Year, Genre, Players, Condition, Price));

            VerifyNewGame();
        }

        // Method for verifying a newly created game. 
        // Make into a more generic method to print verifying information on a single game for both AddGame() UpdateGame() and DeleteGame() taking ID as parameter?
        // Ved AddGame() er spil ID'et gamesList.Count, ved UpdateGame(); sættes ID i metoden, og ved DeleteGame() er al information lige blevet slettet for ID'et...
        public void VerifyNewGame()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine($"You have succesfully added the new game, {gamesList[gamesList.Count - 1].Title}!");

                Console.WriteLine("\nSummary:\n");
                Console.WriteLine($"Title: {gamesList[gamesList.Count - 1].Title}");
                Console.WriteLine($"Year: {gamesList[gamesList.Count - 1].Year}");
                Console.WriteLine($"Genre: {gamesList[gamesList.Count - 1].Genre}");
                Console.WriteLine($"Players: {gamesList[gamesList.Count - 1].Players}");
                Console.WriteLine($"Condition: {gamesList[gamesList.Count - 1].Condition}");
                Console.WriteLine($"Price: {gamesList[gamesList.Count - 1].Price}\n");

                Console.WriteLine("Press <enter> to return to the menu.");
                Console.ReadLine();
                Console.Clear();
                AddEditRemoveMenu();
            }
        }

        // Method to Read (Shows information for each game in gamesList).
        public void ShowGames()
        {
            Console.WriteLine("Games overview: \n");
            foreach (Game game in gamesList)
            {
                Console.WriteLine($"Game Entry no.: {i}");
                Console.WriteLine($"Title: {game.Title}");
                Console.WriteLine($"Year: {game.Year}");
                Console.WriteLine($"Genre: {game.Genre}");
                Console.WriteLine($"Players (x-y): {game.Players}");
                Console.WriteLine($"Condition: {game.Condition}");
                Console.WriteLine($"Price (DKK): {game.Price}");
                // Adds a blank line between each game.
                Console.WriteLine();
                i++;
            }
            Console.WriteLine("Press <enter> to return to menu.");
            Console.ReadLine();
            i = 1;
            return;
        }

        // Method to Update (User selects an already existing game entry in gamesList).
        // Add option to search for game to find the ID of a game to delete? (same issue as in DeleteGame(), user needs to know the ID beforehand).
        // Needs to reset variables: id, editNewGameField and valueNewGameField, after using them.
        public void UpdateGame()
        {
            Console.Clear();

            try
            {
                Console.Write("Please write the ID of the game to update: ");
                int.TryParse(Console.ReadLine(), out id);

                Console.WriteLine($"You have selected {gamesList[id - 1].Title} to update. Is this correct? (Y/N)");
                verifyChoice = Console.ReadLine().ToLower();
            }
            catch (Exception)
            {
                Console.WriteLine("You entered something invalid. Press <enter> to try again.");
                Console.ReadLine();
                Console.Clear();
                UpdateGame();
            }

            if (verifyChoice == "y")
            {
                UpdateField();
                return;
            }
            else
            {
                UpdateGame();
            }

            AddEditRemoveMenu();
        }

        // Method for updating fields in UpdateGame() and VerifyNewGame();
        // Add option to update more fields?
        public void UpdateField()
        {
            Console.Clear();

            Console.WriteLine("Summary: \n");
            Console.WriteLine($"Title: {gamesList[id - 1].Title}");
            Console.WriteLine($"Year: {gamesList[id - 1].Year}");
            Console.WriteLine($"Genre: {gamesList[id - 1].Genre}");
            Console.WriteLine($"Players: {gamesList[id - 1].Players}");
            Console.WriteLine($"Condition: {gamesList[id - 1].Condition}");
            Console.WriteLine($"Price: {gamesList[id - 1].Price}");

            Console.Write("\nSelect a field to correct: (Title, Year, etc.): ");
            // Need validation of user input here, valueNewGameField is validated in the switch
            var editNewGameField = Console.ReadLine();
            Console.Write($"Please enter the correct value for {editNewGameField}: ");
            var valueNewGameField = Console.ReadLine();

            try
            {
                switch (editNewGameField.ToLower())
                {
                    case "title":
                        gamesList[id - 1].Title = valueNewGameField;
                        break;
                    case "year":
                        gamesList[id - 1].Year = int.Parse(valueNewGameField);
                        break;
                    case "genre":
                        gamesList[id - 1].Genre = valueNewGameField;
                        break;
                    case "players":
                        gamesList[id - 1].Players = valueNewGameField;
                        break;
                    case "condition":
                        gamesList[id - 1].Condition = int.Parse(valueNewGameField);
                        break;
                    case "price":
                        gamesList[id - 1].Price = int.Parse(valueNewGameField);
                        break;
                    default:
                        Console.WriteLine("Invalid field name. Press <enter> to try again.");
                        Console.ReadLine();
                        UpdateField();
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("You entered something invalid. Press <enter> to try again.");
                Console.ReadLine();
                Console.Clear();
                UpdateField();
            }
            id = 0;

            Console.Clear();
            Console.WriteLine($"You succesfully updated {editNewGameField} to \"{valueNewGameField}\". Press <enter> to return to the menu.");
            Console.ReadLine();
            Console.Clear();
            AddEditRemoveMenu();
        }

        // Method for Delete (Removes a game from the gamesList).
        // Add option to exit to menu if no game is to be deleted.
        // Add option to search for game to find id of game to delete?
        // Deletion must also remove the game in the txt.file, not just the list. Maybe write entire gamesList back out to file after deletion? (overwriting it all).
        public void DeleteGame() 
        {
            try
            {
                Console.Write("Please write the ID of the game to delete: ");
                int.TryParse(Console.ReadLine(), out id);

                Console.WriteLine($"You have selected {gamesList[id-1].Title} for deletion. Is this correct? (Y/N)");
                verifyChoice = Console.ReadLine().ToLower();

                if (verifyChoice == "y")
                {
                    Console.Clear();
                    Console.WriteLine($"You have succesfully deleted the game, {gamesList[id-1].Title}! \n\nPress <enter> to return to menu...");
                    gamesList.RemoveAt(id - 1);
                    Console.ReadLine();
                    verifyChoice = "";
                    AddEditRemoveMenu();
                }
                else
                {
                    Console.Clear();
                    DeleteGame();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("You entered something incorrect. Press <enter> to try again.");
                Console.ReadLine();
                Console.Clear();
                DeleteGame();
            }

        }

    }
}
