using System.Collections.Generic;
using API.Models.DTOModels;
using API.Models.ViewModels;

namespace API.Repositories.GameRepo
{
    public interface IGameRepository
    {
         IEnumerable<GameDTO> getAllGames();
         GameDTO getGameById(int id);
         GameDTO addGame(GameViewModel newGame);
         IEnumerable<GameInfoDTO> getAllGameInfo();
    }
}