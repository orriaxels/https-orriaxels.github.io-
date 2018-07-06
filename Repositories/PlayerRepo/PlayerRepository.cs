using System;
using System.Linq;
using System.Collections.Generic;
using API.Models.DTOModels;
using API.Models.EntityModels;
using API.Models.ViewModels;
using API.Repositories;
using API.Repositories.GameRepo;

namespace API.Repositories.PlayerRepo
{
    public class PlayerRepository : IPlayerRepository
    {

        private AppDataContext _db;
        private IGameRepository _gameRepo;

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
                deleted = false,
            };

            _db.Player.Add(playerEntity);
            _db.SaveChanges();

            return new PlayerDTO
            {
                ID = playerEntity.ID,
                name = playerEntity.name,
                wins = playerEntity.wins,
                losses = playerEntity.losses,
                draws = playerEntity.draws,
                attented = playerEntity.attented,
                gamesLost = playerEntity.gamesLost,
                gamesWon = playerEntity.gamesWon
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
                            attented = p.attented
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
                            attented = p.attented
                        }).SingleOrDefault();
            
            return player;
        }

        public IEnumerable<TeammatesDTO> getTeammates(int pid1, int pid2) 
        {
            var p1 = getPlayerById(pid1);
            var p2 = getPlayerById(pid2);

            var pid1Games = getAllGameInfoByPid(pid1);
            var pid2Games = getAllGameInfoByPid(pid2);

            var games = (from g1 in pid1Games join g2 in pid2Games
                        on g1.gid equals g2.gid
                        where (g1.teamOne == true && g2.teamOne == true) ||                  (g1.teamTwo == true && g2.teamTwo == true)
                        select new TeammatesDTO
                        {
                            gid = g1.gid,
                            p1 = p1.name,
                            p2 = p2.name,
                            teamOneScore = g1.teamOneScore,
                            teamTwoScore = g1.teamTwoScore,
                            teamOne = g1.teamOne,
                            teamTwo = g1.teamTwo,
                            victory = false
                        }).ToList();

            

            foreach(TeammatesDTO g in games)
            {

                g.teamOneList = getTeamOneList(g);
                g.teamTwoList = getTeamTwoList(g);
                
                if(g.teamOne)
                {
                    if(g.teamOneScore > g.teamTwoScore)                
                        g.victory = true;                    
                    else
                        g.victory = false;
                }
                else if(g.teamTwo)
                {
                    if(g.teamTwoScore > g.teamOneScore)
                        g.victory = true;
                    else
                        g.victory = false;
                }
            }
            return games;
        }

        public List<string> getTeamOneList(TeammatesDTO model)
        {
            var nameList = new List<string>();

            var pidList = (from g in _db.GamesWon
                        where (g.gid == model.gid && g.teamOne == true)
                        select g.pid).ToList();

            foreach(int l in pidList)
            {
                var player = getPlayerById(l);
                nameList.Add(player.name);                
            }

            return nameList;
        }

        public List<string> getTeamTwoList(TeammatesDTO model)
        {
            var nameList = new List<string>();

            var pidList = (from g in _db.GamesWon
                        where (g.gid == model.gid && g.teamTwo == true)
                        select g.pid).ToList();

            foreach(int l in pidList)
            {
                var player = getPlayerById(l);
                nameList.Add(player.name);                
            }

            return nameList;
        }

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
            var player = _db.Player.Where(p => p.ID == id).SingleOrDefault();
            if(player != null)
            {
                player.deleted = true;
                _db.SaveChanges();
            }
        }

        public IEnumerable<TeammatesDTO> getBestTeammates()
        {
            return null;
        }
    }
}