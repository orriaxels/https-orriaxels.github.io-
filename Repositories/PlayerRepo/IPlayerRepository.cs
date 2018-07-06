using System.Collections.Generic;
using API.Models.DTOModels;
using API.Models.ViewModels;
using API.Models.EntityModels;

namespace API.Repositories.PlayerRepo
{
    public interface IPlayerRepository
    {
        PlayerDTO addPlayer(PlayerViewModel newPlayer);
        IEnumerable<PlayerDTO> getAllPlayers();
        PlayerDTO getPlayerById(int id);
        PlayerDTO editPlayer(Player playerToBeEdited);
        void deletePlayer(int id);
        
        // Gets all games where pid1 & pid2 are in the same team
        IEnumerable<TeammatesDTO> getTeammates(int pid, int pid2);

        // Gets best teammates over all 
        IEnumerable<TeammatesDTO> getBestTeammates();
        
    }
}