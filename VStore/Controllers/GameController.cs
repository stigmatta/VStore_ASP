using AutoMapper;
using Azure.Core;
using Business_Logic.Services;
using Data_Access.Models;
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
        private readonly IPaginationService<Achievement> _achievementPaginationService;
        private readonly IMapper _mapper;
        private readonly AchievementService _achievementService;

        private readonly ILogger<GameController> _logger;
        public GameController(
            GameService gameService,
            GameGalleryService gameGalleryService,
            ILogger<GameController> logger,
            IPaginationService<Game> paginationService,
            IMapper mapper,
            AchievementService achievementService,
            IPaginationService<Achievement> achievementPaginationService
            )
        {
            _gameService = gameService;
            _gameGalleryService = gameGalleryService;
            _logger = logger;
            _paginationService = paginationService;
            _mapper = mapper;
            _achievementService = achievementService;
            _achievementPaginationService = achievementPaginationService;
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
                _logger.LogWarning($"GetAllInfo request: {request.Id}");
                var minimum = await _gameService.GetMinimumRequirement(request.Id);
                var recommended = await _gameService.GetRecommendedRequirement(request.Id);
                var allAchievements = await _achievementService.GetAll(request.Id);
                var achievements = _mapper.Map<IEnumerable<AchievementDTO>>(allAchievements.Take(4));
                var userId = Guid.Parse(Request.Cookies["userId"]);

                return Ok(new
                {
                    minimum,
                    recommended,
                    userId,
                    achievements,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchRequest request) 
        {
            try
            {
                var games = await _gameService.GetSearchedGames(request.SearchTerm);
                if (games == null || !games.Any())
                    return NotFound("No games found");
                var gameDTO = games.Select(game => new SearchResponse { Id = game.Id, Title = game.Title }).ToList();
                return Ok(gameDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for games");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{gameId}/achievements")]
        public async Task<IActionResult> GetPaginatedAchievements(string gameId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            if (!Guid.TryParse(gameId, out var gameGuid))
                return BadRequest("Invalid game ID");
            var achievements = await _achievementService.GetAll(gameGuid);
            (IEnumerable<Achievement?> gameAchievements, int totalCount) = _achievementPaginationService.Paginate(achievements, pageNumber - 1, pageSize);
            var items = _mapper.Map<IEnumerable<AchievementDTO>>(gameAchievements);
            return Ok(new
            {
                items,
                totalCount,
            });
        }

        public class SearchRequest
        {
            public string SearchTerm { get; set; }
        }

        public class SearchResponse
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
        }


        public class GameRequest
        {
            public Guid Id { get; set; }
        }
    }
}
