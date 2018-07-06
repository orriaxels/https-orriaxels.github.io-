using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models.DTOModels;
using API.Models.ViewModels;
using API.Models.EntityModels;
using API.Services.GameService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class GameController : Controller
    {
        private IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        [Route("api/games", Name = "GetAllGames")]
        public IActionResult GetAllGames()
        {
            IEnumerable<GameDTO> game;

            game = _gameService.getAllGames();

            return Ok(game);
        }

        [HttpGet]
        [Route("api/games/{id:int}", Name = "GetGameById")]
        public IActionResult GetGameById(int id)
        {
            GameDTO game = _gameService.getGameById(id);
            
            return Ok(game);
        }

        [HttpPost]
        [Route("api/games/", Name = "AddGame")]
        public IActionResult AddGame([FromBody]GameViewModel newGame)
        {
            var game = _gameService.addGame(newGame);
            
            return Ok(game);
        }
    }


}