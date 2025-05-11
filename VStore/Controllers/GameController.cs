using Azure.Core;
using Business_Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace VStore.Controllers
{
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        private readonly GameGalleryService _gameGalleryService;
        private readonly ILogger<GameController> _logger;
        public GameController(
            GameService gameService,
            GameGalleryService gameGalleryService,
            ILogger<GameController> logger
            )
        {
            _gameService = gameService;
            _gameGalleryService = gameGalleryService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GetGame([FromBody] GameRequest request) 
        {
            try
            {
                if (request.Id == Guid.Empty)
                    return BadRequest("Invalid game ID");

                var game = await _gameService.GetById(request.Id);
                if (game == null)
                    return NotFound("Game not found");
                var gallery = await _gameGalleryService.GetByGameId(request.Id);
                var gameWithGallery = _gameService.ConnectGameWithGallery(game, gallery);

                return Ok(gameWithGallery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching game");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("get-info")]
        public async Task<IActionResult> GetAllInfo([FromBody] GameRequest request)
        {
            try
            {
                if (request.Id == Guid.Empty)
                    return BadRequest("Invalid game ID");
                var minimum = await _gameService.GetMinimumRequirement(request.Id);
                var recommended = await _gameService.GetRecommendedRequirement(request.Id);
                var userId = Request.Cookies["userId"];
                return Ok(new
                {
                    minimum,
                    recommended,
                    userId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching game requirements");
                return StatusCode(500, "Internal server error");
            }
        }


        public class GameRequest
        {
            public Guid Id { get; set; }
        }
    }
}
