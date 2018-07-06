using System;

namespace API.Models.EntityModels
{
    public class Player
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public int draws { get; set; }
        public int gamesWon { get; set; }
        public int gamesLost { get; set; }
        public int attented { get; set; }
        public bool deleted { get; set; }
    }
}
