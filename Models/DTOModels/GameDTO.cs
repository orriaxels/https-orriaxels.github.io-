using System;

namespace API.Models.DTOModels
{
    public class GameDTO
    {
        public int ID { get; set; }
        public bool teamOneWin { get; set; }
        public bool teamTwoWin { get; set; }
        public bool draw { get; set; }
        public string teamOneList { get; set; }
        public string teamTwoList { get; set; }
        public DateTime date { get; set; }
    }
}
