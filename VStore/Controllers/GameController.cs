using AutoMapper;
using Azure.Core;
using Business_Logic.Services;
using Data_Transfer_Object.DTO.Achievement;
using Microsoft.AspNetCore.Mvc;

namespace VStore.Controllers
{
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        private readonly GameGalleryService _gameGalleryService;
        private readonly IPaginationService<Game> _paginationService;
        private readonly IMapper _mapper;
        private readonly AchievementService _achievementService;

        private readonly ILogger<GameController> _logger;
        public GameController(
            GameService gameService,
            GameGalleryService gameGalleryService,
            ILogger<GameController> logger,
            IPaginationService<Game> paginationService,
            IMapper mapper,
            AchievementService achievementService
            )
        {
            _gameService = gameService;
            _gameGalleryService = gameGalleryService;
            _logger = logger;
            _paginationService = paginationService;
            _mapper = mapper;
            _achievementService = achievementService;
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
                var userId = Guid.Parse(Request.Cookies["userId"]);
                var achievements = _mapper.Map<List<AchievementDTO>>(await _achievementService.GetAll(request.Id));
                return Ok(new
                {
                    minimum,
                    recommended,
                    userId,
                    achievements
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
