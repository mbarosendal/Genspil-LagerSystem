using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektGenspil
{
    public class Game
    {
        private static int nextId = 1;
        public int Id { get; private set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Players { get; set; }
        public int Condition { get; set; }
        public int Price { get; set; }
        public bool Stock { get; set; }
        public bool Requested { get; set; }
        public string RequestedBy { get; set;}

        // The constructor.
        public Game(string title, int year, string genre, string players, int condition, int price, bool stock, bool requested, string requestedBy)
        {
            this.Id = nextId++;
            this.Title = title/*.ToLower() for searching?*/;
            this.Year = year;
            this.Genre = genre;
            this.Players = players;
            this.Condition = condition;
            this.Price = price;
            this.Stock = stock;
            this.Requested = requested;
            this.RequestedBy = requestedBy;
        }
    }
}
