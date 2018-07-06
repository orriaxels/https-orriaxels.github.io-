using System;
using System.Collections.Generic;

namespace API.Models.EntityModels
{
    public class Game
    {
        public int ID { get; set; }
        public bool teamOneWin { get; set; }
        public bool teamTwoWin { get; set; }
        public bool draw { get; set; }
        public string date { get; set; }
        public bool deleted { get; set; }
    }
}
