using System.Collections.Generic;
using API.Models.DTOModels;
using API.Models.EntityModels;
using API.Models.ViewModels;
using System.Linq;
using API.Repositories.PlayerRepo;

namespace API.Repositories.GameRepo
{
    public class GameRepository : IGameRepository
    {
        private AppDataContext _db;

        public GameRepository(AppDataContext db)
        {
            _db = db;
        }

        public IEnumerable<GameDTO> getAllGames()
        {
            var game = (from g in _db.Game
                        where g.deleted == false
                        select new GameDTO
                        {
                            ID = g.ID,
                            date = g.date.Date,
                            teamOneWin = g.teamOneWin,
                            teamTwoWin = g.teamTwoWin
                        }).ToList();

            return game;
        }

        public GameDTO addGame(GameViewModel newGame)
        {             
            var gameEntity = new Game
            {
                date = newGame.date,
                teamOneWin = false,
                teamTwoWin = false,
                teamOneList = "",
                teamTwoList = "",
                draw = false,
                deleted = false
            };

            if(newGame.teamOneScore > newGame.teamTwoScore)
            {
                gameEntity.teamOneWin = true;
                }
            else if(newGame.teamOneScore < newGame.teamTwoScore)
            {
                gameEntity.teamTwoWin = true;
            }
            else
            {
                gameEntity.draw = true;
            }            

            _db.Game.Add(gameEntity);
            _db.SaveChanges();

            addGameInfo(newGame, gameEntity.ID);
            return new GameDTO
            {                
                teamOneWin = gameEntity.teamOneWin,
                teamTwoWin = gameEntity.teamTwoWin,
                draw = gameEntity.draw,
                teamOneList = gameEntity.teamOneList,
                teamTwoList = gameEntity.teamTwoList,
                date = gameEntity.date            
            };
        }

        public void updatePlayer(int pid, GameViewModel newGame)
        {
            var player = _db.Player.Find(pid);

            if(checkWhichTeam(pid, newGame))
            {                
                if(newGame.teamOneScore > newGame.teamTwoScore)
                {
                    player.wins++;
                    player.lastFive += "w,";
                }
                else
                {
                    player.losses++;                
                    player.lastFive += "l,";
                }
            }
            else
            {                
                if(newGame.teamTwoScore > newGame.teamOneScore) 
                {
                    player.wins++;
                    player.lastFive += "w,";
                }
                else
                {
                    player.losses++;                
                    player.lastFive += "l,";
                }
            }

            player.gamesWon += newGame.teamOneScore;
            player.gamesLost += newGame.teamTwoScore;

            player.attented++;
            _db.Player.Update(player);
        }

        public bool checkWhichTeam(int pid, GameViewModel newGame)
        {
            var teamOne = newGame.teamOneList;
            
            if(teamOne.Contains(pid))
                return true;
            else
                return false;
        }

        public void addGameInfo(GameViewModel newGame, int gid)
        {
            var teamOne = newGame.teamOneList;
            var teamTwo = newGame.teamTwoList;
            
            foreach(int pid in teamOne)
            {
                var gameInfo = new GameInfo
                {
                    gid = gid,
                    pid = pid,
                    teamOneScore = newGame.teamOneScore,
                    teamTwoScore = newGame.teamTwoScore,
                    result = "",
                    teamOne = true,
                    teamTwo = false,
                    draw = false
                };
                updatePlayer(pid, newGame);
                _db.GamesWon.Add(gameInfo);
            }

            foreach(int pid in teamTwo)
            {
                var gameInfo = new GameInfo
                {
                    gid = gid,
                    pid = pid,
                    teamOneScore = newGame.teamOneScore,
                    teamTwoScore = newGame.teamTwoScore,
                    result = "",
                    teamOne = false,
                    teamTwo = true,
                    draw = false
                };
                updatePlayer(pid, newGame);
                _db.GamesWon.Add(gameInfo);
            }
            
            _db.SaveChanges();
        }

        public GameDTO getGameById(int id)
        {
            var game = (from g in _db.Game
                        where g.ID == id && g.deleted == false
                        select new GameDTO
                        {
                            ID = g.ID,
                            date = g.date,
                            teamOneWin = g.teamOneWin,
                            teamTwoWin = g.teamTwoWin
                        }).SingleOrDefault();

            return game;
        }

        public IEnumerable<GameInfoDTO> getAllGameInfo()
        {
            var gameInfo = (from g in _db.GamesWon
                            select new GameInfoDTO
                            {
                                ID = g.ID,
                                gid = g.gid,
                                pid = g.pid,
                                teamOneScore = g.teamOneScore,
                                teamTwoScore = g.teamTwoScore,
                                result = g.result,
                                teamOne = g.teamOne,
                                teamTwo = g.teamTwo
                            }).ToList();

            return gameInfo;
        }
    }
}