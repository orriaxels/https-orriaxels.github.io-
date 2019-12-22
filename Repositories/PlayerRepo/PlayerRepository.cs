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
            var total = Stopwatch.StartNew();
            int wins = 0;
            int losses = 0;
            
            var SWgetPlayerById = Stopwatch.StartNew();
            var p1 = getPlayerName(pid1);
            var p2 = getPlayerName(pid2);
            SWgetPlayerById.Stop();
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine(p1 + " og " + p2);

            var SWgetAllGameInfoByPid = Stopwatch.StartNew();
            var pid1Games = getAllGameInfoByPid(pid1);
            var pid2Games = getAllGameInfoByPid(pid2);
            SWgetAllGameInfoByPid.Stop();

            var SWgame = Stopwatch.StartNew();
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
            SWgame.Stop();

            var SWgameInGame = Stopwatch.StartNew();
            foreach(TeammatesGames game in games)
            {                
                var getTeamList1 = Stopwatch.StartNew();
                game.teamOneList = getTeamOneList(game.gid);
                getTeamList1.Stop();
               
                game.teamTwoList = getTeamTwoList(game.gid);

                Console.WriteLine("get teamList1: {0}", getTeamList1.Elapsed); 

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
            SWgameInGame.Stop();

            var teammates = new TeammatesDTO
            {
                p1 = p1,
                p2 = p2,
                wins = wins,
                losses = losses,
                count = games.Count,
                TeammatesGames = games
            };
            total.Stop();
            

            Console.WriteLine("getPlayerById: {0}", SWgetPlayerById.Elapsed); 
            Console.WriteLine("getAllGameInfoByPid: {0}", SWgetAllGameInfoByPid.Elapsed); 
            Console.WriteLine("get games: {0}", SWgame.Elapsed); 
            Console.WriteLine("foreach game in games: {0}", SWgameInGame.Elapsed); 
            Console.WriteLine("Total time: {0}", total.Elapsed); 
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine();

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
            int nrOfPlayers = getNrOfPlayers();

            var wins = 0;
            TeammatesDTO tempTeammates = new TeammatesDTO();

            for(int i = 1; i <= nrOfPlayers; i++)
            {
                if(i == pid)
                    continue;
                
                var games = getTeammates(pid, i);
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
            int nrOfPlayers = getNrOfPlayers();

            var losses = 0;
            TeammatesDTO tempTeammates = new TeammatesDTO();

            for(int i = 1; i <= nrOfPlayers; i++)
            {
                if(i == pid)
                    continue;
                
                var games = getTeammates(pid, i);
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
            var totaltime = Stopwatch.StartNew();
            int nrOfPlayers = getNrOfPlayers();            

            var wins = 0;
            TeammatesDTO tempTeammates = new TeammatesDTO();
            for(int i = 1; i <= nrOfPlayers; i++)
            {                
                var stopwatch = Stopwatch.StartNew();
                for(int j = 1; j <= nrOfPlayers; j++)
                {   
                    if(i == j || j < i)
                        continue;
                    
                    var games = getTeammates(i, j);
                    int temp = games.wins;
                    if(temp > wins)
                    {
                        wins = games.wins;                
                        tempTeammates = games;
                    }
                }
                stopwatch.Stop();
                
                Console.WriteLine("id: {0} - {1}", i, stopwatch.Elapsed);                 
            }

            totaltime.Stop();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Total time: {0}", totaltime.Elapsed);                 
            return tempTeammates;
        }

        public TeammatesDTO overallWorstTeammates()
        {
            int nrOfPlayers = getNrOfPlayers();

            var losses = 0;
            TeammatesDTO tempTeammates = new TeammatesDTO();
            for(int i = 1; i <= nrOfPlayers; i++)
            {
                for(int j = 1; j <= nrOfPlayers; j++)
                {   
                    if(i == j || j < i)
                        continue;
                    
                    var games = getTeammates(i, j);
                    int temp = games.losses;
                    if(temp > losses)
                    {
                        losses = games.losses;                
                        tempTeammates = games;
                    }
                }
            }

            return tempTeammates;
        }

        private int getNrOfPlayers()
        {            
            return _db.Player.ToList().Count;
        }
    }
}