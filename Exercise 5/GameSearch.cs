using ProjektGenspil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_5
{
    public class GameSearch
    {
        private GameRepository gameRepository;

        public GameSearch(GameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
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

            if (gameRepository.gamesList.Count == 0)
            {
                Console.WriteLine("The list is empty!");
                Console.ReadLine();
            }

            try
            {
                switch (criteria)
                {
                    case 1:
                        // Lambda expression to search through objects in gamesList, and look for one, where the Title property contains the searchValue. Return found as a list, since searchResults is a list.
                        searchResults = gameRepository.gamesList.Where(game => game.Title.ToLower().Contains(searchValue)).ToList();
                        break;
                    case 2:
                        searchResults = gameRepository.gamesList.Where(game => game.Genre.ToLower().Contains(searchValue)).ToList();
                        break;
                    case 3:
                        searchResults = gameRepository.gamesList.Where(game => game.Players.ToLower().Contains(searchValue)).ToList();
                        break;
                    case 4:
                        int condition = int.Parse(searchValue);
                        searchResults = gameRepository.gamesList.Where(game => game.Condition == condition).ToList();
                        break;
                    case 5:
                        int price = int.Parse(searchValue);
                        searchResults = gameRepository.gamesList.Where(game => game.Price == price).ToList();
                        break;
                    case 6:
                        searchResults = gameRepository.gamesList.Where(game => game.Stock == true).ToList();
                        break;
                    case 7:
                        searchResults = gameRepository.gamesList.Where(game => game.Requested == true).ToList();
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
                    ConsoleUI.ShowGames(searchResults);
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
 