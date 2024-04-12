using ProjektGenspil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_5
{
    public class GameRepository
    {
        public List<Game> gamesList = new List<Game>();

        // String for verifying user choice in VerifyNewGame() and DeleteGame().
        string verifyChoice;

        //Method to load the games from a txt.file(GenspilGames.txt) into a list(gamesList) as setup.
        public void LoadGamesFromFile()
        {
            try
            {
                string directoryPath = @"C:\Users\mbaro\Desktop";
                string fileName = "GenspilGames.txt";
                string filePath = Path.Combine(directoryPath, fileName);

                // Loads a pre-set file with a set path.
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] gameData = line.Split(','); // Assuming comma-separated values.                              

                        if (gameData.Length == 9) // Ensure all nine fields are present in each line (since the gameData array with the split data should have nine indexes).
                        {
                            // Parse each piece of information from the file from each index of the gameData array.
                            string title = gameData[0];
                            int year = int.Parse(gameData[1]);
                            string genre = gameData[2];
                            string players = gameData[3];
                            int condition = int.Parse(gameData[4]);
                            int price = int.Parse(gameData[5]);
                            bool stock = bool.Parse(gameData[6]);
                            bool requested = bool.Parse(gameData[7]);
                            string requestedBy = gameData[8];

                            // Add the new Game instance to the gamesList.
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
                Console.ReadLine();
                return;
            }
        }


        public Game GetGameById()
        {
            Console.Write("Please write the ID of the game: ");
            int.TryParse(Console.ReadLine(), out int id);

            Game game = gamesList.FirstOrDefault(game => game.Id == id);

            Console.WriteLine(game.Id);
            Console.WriteLine(game.Title);
            Console.ReadLine();

            if (game == null)
            {
                Console.WriteLine($"Game with ID {id} not found. Press <enter> to return to menu.");
                Console.ReadLine();
                return null;
            }

            return game;
        }
        // A method for retrieivng a game by its ID and returning that specific game object.
        //public Game GetGameById(List<Game> gamesList)
        //{
        //    int id;
        //    Console.Write("Please write the ID of the game: ");
        //    int.TryParse(Console.ReadLine(), out id);

        //    // Search for the game with the parameter IDs.
        //    Game game = gamesList.FirstOrDefault(game => game.Id == id);

        //    if (game == null)
        //    {
        //        Console.WriteLine($"Game with ID {id} not found. Press <enter> to return to menu.");
        //        Console.ReadLine();
        //        return null;
        //    }

        //    return game;
        //}


        //Alternative method to load a public.txt file from Google Drive(but saving cannot be done using this method, needs API?).
        //    public async Task LoadGamesFromFile()
        //    {
        //    try
        //    {
        //        string fileUrl = "https://drive.usercontent.google.com/download?id=1S8L4-vwd3eV712DxFQpZ6qtJJsaTY1w2";

        //        // Download the contents of the text file through HttpClient method
        //        using (HttpClient client = new HttpClient())
        //        {
        //            // Download the content of the text file to a string 
        //            string fileContents = await client.GetStringAsync(fileUrl);

        //            // Split the content into lines
        //            string[] lines = fileContents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        //            foreach (string line in lines)
        //            {
        //                string[] gameData = line.Split(','); // Assuming comma-separated values.

        //                if (gameData.Length == 9) // Ensure all nine fields are present in each line
        //                {
        //                    // Parse each piece of information from the file
        //                    string title = gameData[0];
        //                    int year = int.Parse(gameData[1]);
        //                    string genre = gameData[2];
        //                    string players = gameData[3];
        //                    int condition = int.Parse(gameData[4]);
        //                    int price = int.Parse(gameData[5]);
        //                    bool stock = bool.Parse(gameData[6]);
        //                    bool requested = bool.Parse(gameData[7]);
        //                    string requestedBy = gameData[8];

        //                    // Add the new Game instance to the gamesList
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
        //    }
        //}


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

            ConsoleUI.GameSummary(gamesList[gamesList.Count - 1]);
            Console.Write("\nPress <enter> to continue.");
            Console.ReadLine();
            return;
        }

        // Method to Update a game object. Instead make into generic verify choice method for several methods to use?
        // Just integrate directly into UpdateField()?
        public void UpdateGame()
        {
            Console.Clear();

            Game game = GetGameById();

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

            //consoleUI.AddEditRemoveMenu();
        }

        // Method for updating fields in UpdateGame() and VerifyNewGame();
        public void UpdateField(Game game)
        {
            Console.Clear();
            string fieldValue = "";

            ConsoleUI.GameSummary(game);

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

            ConsoleUI.GameSummary(game);
            Console.Write($"\n{game.Title} was succesfully updated. \nPress <enter> to continue.\n");
            Console.ReadLine();
            return;
        }

        // Method for Delete (Removes a game from the gamesList).
        // Deletion must also remove the game in the txt.file, not just the list? Maybe write entire gamesList back out to file after deletion? (overwriting it all) or just have separate save option in main menu.
        public void DeleteGame()
        {
            try
            {
                Game game = GetGameById();

                ConsoleUI.GameSummary(game);

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
                        //consoleUI.AddEditRemoveMenu();
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
