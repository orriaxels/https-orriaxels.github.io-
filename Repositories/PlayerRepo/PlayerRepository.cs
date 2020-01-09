using System.Linq;
using System.Collections.Generic;
using API.Models.DTOModels;
using API.Models.EntityModels;
using API.Models.ViewModels;
using System;
using System.Diagnostics;


namespace API.Repositories.PlayerRepo
{
    public class PlayerRepository : IPlayerRepository
    {

        private AppDataContext _db;        

        public PlayerRepository(AppDataContext db)
        {
            _db = db;            
        }

        public PlayerDTO addPlayer(PlayerViewModel newPlayer)
        {           
            var playerEntity = new Player
            {
                name = newPlayer.name,
                wins = 0,
                losses = 0,
                draws = 0,
                attented = 0,
                gamesLost = 0,
                gamesWon = 0,
                lastFive = "",
                deleted = false,
            };

            _db.Player.Add(playerEntity);
            _db.SaveChanges();

            return new PlayerDTO
            {
                name = playerEntity.name,
                wins = playerEntity.wins,
                losses = playerEntity.losses,
                draws = playerEntity.draws,
                attented = playerEntity.attented,
                gamesLost = playerEntity.gamesLost,
                gamesWon = playerEntity.gamesWon,
                lastFive = ""
            };
        }

        public IEnumerable<PlayerDTO> getAllPlayers()
        {
            var player = (from p in _db.Player
                        where p.deleted != true
                        select new PlayerDTO
                        {
                            ID = p.ID,
                            name = p.name,
                            wins = p.wins,
                            losses = p.losses,
                            draws = p.draws,
                            gamesLost = p.gamesLost,
                            gamesWon = p.gamesWon,
                            attented = p.attented,
                            lastFive = p.lastFive
                        }).ToList();
            
            return player;
        }        

        public PlayerDTO getPlayerById(int id)
        {            
            var player = (from p in _db.Player
                        where p.ID == id && p.deleted == false
                        select new PlayerDTO
                        {
                            ID = p.ID,
                            name = p.name,
                            wins = p.wins,
                            losses = p.losses,
                            draws = p.draws,
                            gamesLost = p.gamesLost,
                            gamesWon = p.gamesWon,
                            attented = p.attented,
                            lastFive = p.lastFive
                        }).SingleOrDefault();
            
            return player;
        }

        private string getPlayerName(int id)
        {
            var x = _db.Player.Find(id);

            return x.name;
        }

        public TeammatesDTO getTeammates(int pid1, int pid2) 
        {
            int wins = 0;
            int losses = 0;
            
            var p1 = getPlayerName(pid1);
            var p2 = getPlayerName(pid2);

            var pid1Games = getAllGameInfoByPid(pid1);
            var pid2Games = getAllGameInfoByPid(pid2);

            var games = (from g1 in pid1Games join g2 in pid2Games
                        on g1.gid equals g2.gid
                        where (g1.teamOne == true && g2.teamOne == true) || (g1.teamTwo == true && g2.teamTwo == true)
                        select new TeammatesGames
                        {
                            gid = g1.gid,
                            teamOneScore = g1.teamOneScore,
                            teamTwoScore = g1.teamTwoScore,
                            teamOne = g1.teamOne,
                            teamTwo = g1.teamTwo,
                            victory = false
                        }).ToList();

            foreach(TeammatesGames game in games)
            {                
                game.teamOneList = getTeamOneList(game.gid);
                game.teamTwoList = getTeamTwoList(game.gid);

                if(game.teamOne)
                {
                    if(game.teamOneScore > game.teamTwoScore)
                    {
                        game.victory = true;
                        wins++;                  
                    }                
                    else
                    {
                        losses++;
                    }
                }
                else
                {
                    if(game.teamTwoScore > game.teamOneScore)
                    {
                        game.victory = true;
                        wins++;
                    }
                    else
                    {
                        losses++;
                    }
                }
            }

            var teammates = new TeammatesDTO
            {
                p1 = p1,
                p2 = p2,
                wins = wins,
                losses = losses,
                count = games.Count,
                TeammatesGames = games
            };

            return teammates;
        }

        public List<string> getTeamOneList(int gid)
        {
            var nameList = new List<string>();

            var pidList = (from g in _db.GamesWon
                        where (g.gid == gid && g.teamOne == true)
                        select g.pid).ToList();

            foreach(var l in pidList)
            {
                nameList.Add(getPlayerName(l));
            }

            return nameList;
        }

        public List<string> getTeamTwoList(int gid)
        {
            var nameList = new List<string>();

            var pidList = (from g in _db.GamesWon
                        where (g.gid == gid && g.teamTwo == true)
                        select g.pid).ToList();

            foreach(var l in pidList)
            {
                nameList.Add(getPlayerName(l));                
            }

            return nameList;
        }


        // helper function to find gameInfoByPid
        public IEnumerable<GameInfoDTO> getAllGameInfoByPid(int pid)
        {            
            var gameInfo = (from g in _db.GamesWon
                            where pid == g.pid
                            select new GameInfoDTO
                            {
                                ID = g.ID,
                                gid = g.gid,
                                pid = pid,
                                teamOneScore = g.teamOneScore,
                                teamTwoScore = g.teamTwoScore,
                                result = g.result,
                                teamOne = g.teamOne,
                                teamTwo = g.teamTwo               
                            }).ToList();
            
            return gameInfo;
        }

        public PlayerDTO editPlayer(Player playerToBeEdited)
        {
            _db.Player.Update(playerToBeEdited);
            _db.SaveChanges();

            var playerDTO = getPlayerById(playerToBeEdited.ID);
            
            return playerDTO;
        }

        public void deletePlayer(int id)
        {
            var player = _db.Player.Find(id);
            if(player != null)
            {
                player.deleted = true;
                _db.SaveChanges();
            }
        }

        public TeammatesDTO getBestTeammate(int pid)
        {
            var playersId = getAllPlayersId();

            var wins = 0;
            TeammatesDTO tempTeammates = new TeammatesDTO();

            foreach(var pid2 in playersId)
            {
                if(pid == pid2)
                    continue;
                
                var games = getTeammates(pid, pid2);
                int temp = games.wins;
                if(temp > wins)
                {
                    wins = games.wins;                
                    tempTeammates = games;
                }
            }

            return tempTeammates;
        }

        public TeammatesDTO getWorstTeammate(int pid)
        {
            var playersId = getAllPlayersId();

            var losses = 0;
            TeammatesDTO tempTeammates = new TeammatesDTO();

            foreach(var pid2 in playersId)
            {
                if(pid == pid2)
                    continue;
                
                var games = getTeammates(pid, pid2);
                int temp = games.losses;
                if(temp > losses)
                {
                    losses = games.losses;                
                    tempTeammates = games;
                }
            }

            return tempTeammates;
        }

        public TeammatesDTO overallBestTeammates()
        {
            var pidList = getAllPlayersId();
            
            TeammatesDTO tempTeammates = new TeammatesDTO();

            foreach(var pid in pidList)
            {
                tempTeammates = getBestTeammate(pid);
            }
               
            return tempTeammates;
        }

        public TeammatesDTO overallWorstTeammates()
        {
            var playersId = getAllPlayersId();
            TeammatesDTO tempTeammates = new TeammatesDTO();

            foreach(var pid in playersId)
            {
                tempTeammates = getWorstTeammate(pid)
            }

            return tempTeammates;
        }

        private List<int> getAllPlayersId()
        {
            return (from p in _db.Player select p.ID).ToList();                            
        }
    }
}