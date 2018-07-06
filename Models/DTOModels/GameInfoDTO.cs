using System;
using System.Collections.Generic;

namespace API.Models.DTOModels
{
  public class GameInfoDTO
  {
    public int ID { get; set; }
    public int gid { get; set; }
    public int pid { get; set; }
    public int teamOneScore { get; set; }
    public int teamTwoScore { get; set; }
    public string result { get; set; }  
    public bool teamOne { get; set; } // true if pid was in teamOne
    public bool teamTwo { get; set; } // true if pid was in teamTwo
    public List<int> teamOneList { get; set; }
    public List<int> teamTwoList { get; set; }
  }
}
