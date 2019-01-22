using System.Collections.Generic;
using API.Models.EntityModels;
using API.Models.DTOModels;
using API.Models.ViewModels;
using API.Repositories.PlayerRepo;

namespace API.Services.PlayerService
{
    public class PlayerService : IPlayerService
    {
        private IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public PlayerDTO addPlayer(PlayerViewModel newPlayer)
        {
            var player = _playerRepository.addPlayer(newPlayer);

            return player;
        }

        public PlayerDTO editPlayer(int id, EditPlayerViewModel updatedPlayer)
        {
            var oldPlayer = _playerRepository.getPlayerById(id);

            var playerToBeEdited = new Player
            {
                ID = oldPlayer.ID,
                name = oldPlayer.name,
                wins = oldPlayer.wins,
                losses = oldPlayer.losses,
                gamesLost = oldPlayer.gamesLost,
                gamesWon = oldPlayer.gamesWon,
                attented = oldPlayer.attented
            };

            if(updatedPlayer.name != null)
            {
                playerToBeEdited.name = updatedPlayer.name;
            }
        
            playerToBeEdited.wins = updatedPlayer.wins;    
            playerToBeEdited.losses = updatedPlayer.losses;
            playerToBeEdited.gamesLost = updatedPlayer.gamesLost;
            playerToBeEdited.gamesWon = updatedPlayer.gamesWon;
            playerToBeEdited.attented = updatedPlayer.attented;

            var player = _playerRepository.editPlayer(playerToBeEdited);

            return player;            
        }

        public IEnumerable<PlayerDTO> getAllPlayers()
        {
            var players = _playerRepository.getAllPlayers();
            
            return players;
        }

        public PlayerDTO getPlayerById(int id)
        {
            var player = _playerRepository.getPlayerById(id);

            return player;
        }

        public void deletePlayer(int id)
        {
            _playerRepository.deletePlayer(id);
        }

        public TeammatesDTO getTeammates(int pid, int pid2)
        {
            var games = _playerRepository.getTeammates(pid, pid2);

            return games;            
        }

        public TeammatesDTO getBestTeammate(int pid)
        {
            var games = _playerRepository.getBestTeammate(pid);

            return games;
        }

        public TeammatesDTO getWorstTeammate(int pid)
        {
            var games = _playerRepository.getWorstTeammate(pid);

            return games;
        }

        public TeammatesDTO overallBestTeammates()
        {
            var games = _playerRepository.overallBestTeammates();

            return games;
        }

        public TeammatesDTO overallWorstTeammates()
        {
            var games = _playerRepository.overallWorstTeammates();

            return games;
        }

    }
}