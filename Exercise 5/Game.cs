using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektGenspil
{
    public class Game
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Players { get; set; }
        public int Condition { get; set; }
        public int Price { get; set; }
        //public bool Stock { get; set; } // set stock status, if added: add a 7th data field to ALL methods... in StreamReader and in GenspilGames.txt (true/false)
        //public bool Requested { get; set } // set if game has been requested, if added: add an 8th data field to ALL methods...^^ (true/false)

        // The constructor.
        public Game(string title, int year, string genre, string players, int condition, int price/*, int stock*/)
        {
            this.Title = title;
            this.Year = year;
            this.Genre = genre;
            this.Players = players;
            this.Condition = condition;
            this.Price = price;
            //this.Stock = stock;
        }
    }
}
