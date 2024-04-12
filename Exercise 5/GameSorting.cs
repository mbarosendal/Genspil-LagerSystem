using ProjektGenspil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_5
{
    public class GameSorting
    {
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
    }
}
