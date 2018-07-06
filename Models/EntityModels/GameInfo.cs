using System;

namespace API.Models.EntityModels
{
  public class GameInfo
  {
    public int ID { get; set; }
    public int gid { get; set; } // gameID
    public int pid { get; set; } // playerID
    public int teamOneScore { get; set; } // how many games teamOne won
    public int teamTwoScore { get; set; } // how many games teamTwo won
    public string result { get; set; } // result represented by a char "w", "l", "d"
    public bool teamOne { get; set; } // true if pid was in teamOne
    public bool teamTwo { get; set; } // true if pid was in teamTwo
    public bool draw { get; set; } // true if teamOneScore == teamTwoScore
  }
}
