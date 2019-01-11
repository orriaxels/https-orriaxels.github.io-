using System.Collections.Generic;
using API.Models.DTOModels;
using API.Models.ViewModels;
using API.Repositories.GameRepo;

namespace API.Services.GameService
{
    public class GameService : IGameService
    {

        private IGameRepository _gameRepository;
        
        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        
        public IEnumerable<GameDTO> getAllGames()
        {
            var games = _gameRepository.getAllGames();

            return games;
        }

        public GameDTO getGameById(int id)
        {
            var game = _gameRepository.getGameById(id);

            return game;
        }

        public GameDTO addGame(GameViewModel newGame)
        {
           var game = _gameRepository.addGame(newGame);

           return game;
        }
        
        public IEnumerable<GameInfoDTO> getAllGameInfo()
        {
            var games = _gameRepository.getAllGameInfo();

            return games;
        }
    }
}