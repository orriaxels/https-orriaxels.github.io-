using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models.DTOModels;
using API.Models.ViewModels;
using API.Models.EntityModels;
using API.Services.PlayerService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        // GET api/values
        [HttpPost]
        [Route("api/players/", Name = "AddPlayer")]
        public IActionResult AddPlayer([FromBody]PlayerViewModel newPlayer)
        {
            var player = _playerService.addPlayer(newPlayer);
            return Ok(player);
        }

        // GET api/values/5
        [HttpGet]
        [Route("api/players", Name = "GetAllPlayers")]
        public IActionResult GetAllPlayers()
        {
            IEnumerable<PlayerDTO> player;

            player = _playerService.getAllPlayers();

            return Ok(player);
        }

        [HttpGet]
        [Route("api/players/{id:int}", Name = "GetPlayerById")]
        public IActionResult GetPlayerById(int id)
        {
            PlayerDTO player = _playerService.getPlayerById(id);
            
            return Ok(player);
        }

        [HttpGet]
        [Route("api/players/{pid1:int}/{pid2:int}", Name = "GetTeammatesGames")]
        public IActionResult GetTeammatesGames(int pid1, int pid2)
        {        
            IEnumerable<TeammatesDTO> games = _playerService.getTeammates(pid1, pid2);

            return Ok(games);

        }

        [HttpPut]
        [Route("api/players/{id:int}", Name = "EditPlayerById")]
        public IActionResult EditPlayerById(int id, [FromBody] EditPlayerViewModel updatedPlayer)
        {
            if(updatedPlayer == null)
            {
                return BadRequest();
            }
            
            if(!ModelState.IsValid)
            {
                return StatusCode(412);
            }

            var player = _playerService.editPlayer(id, updatedPlayer);

            return Ok(player);
        }

        [HttpDelete]
        [Route("api/players/{id:int}", Name = "DeletePlayerById")]
        public IActionResult DeletePlayerById(int id)
        {
            _playerService.deletePlayer(id);
            return NoContent();
        }
        
    }
}
