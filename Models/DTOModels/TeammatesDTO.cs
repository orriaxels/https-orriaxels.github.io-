using System.Collections.Generic;

namespace API.Models.DTOModels
{
    public class TeammatesDTO
    {
        public int gid { get; set; }
        public string p1 { get; set; }
        public string p2 { get; set; }
        public int teamOneScore { get; set; }
        public int teamTwoScore { get; set; }
        public bool teamOne { get; set; }
        public bool teamTwo { get; set; }
        public List<string> teamOneList { get; set; }
        public List<string> teamTwoList { get; set; }
        public bool victory { get; set; }
    }
}