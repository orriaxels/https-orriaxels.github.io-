using System;
using System.Collections.Generic;
using API.Models.EntityModels;

namespace API.Models.ViewModels
{
    public class GameViewModel
    {
        public int ID { get; set; }
        public DateTime date { get; set; }
        public int teamOneScore { get; set; }
        public int teamTwoScore { get; set; }
        public List<int> teamOneList { get; set; }
        public List<int> teamTwoList { get; set; }
    }
}