using System.Collections.Generic;

namespace API.Models.DTOModels
{
    public class TeammatesDTO
    {
        public string p1 { get; set; }
        public string p2 { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public int count { get; set; }
        public List<TeammatesGames> TeammatesGames { get; set; }
    }
    public class TeammatesGames
    {
        public int gid { get; set; }
        public int teamOneScore { get; set; }
        public int teamTwoScore { get; set; }
        public bool teamOne { get; set; }
        public bool teamTwo { get; set; }
        public List<string> teamOneList { get; set; }
        public List<string> teamTwoList { get; set; }
        public bool victory { get; set; }
    }
}