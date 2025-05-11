using AutoMapper;
using Business_Logic.Services;
using Data_Access.Models;
using Data_Transfer_Object.DTO.GameDTO;
using Data_Transfer_Object.DTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VStore.Controllers
{
    [Authorize(Policy = "CookieUserPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly UserGamesService _userGamesService;
        private readonly IPaginationService<UserGame> _paginationService;

        public ProfileController(
            ILogger<ProfileController> logger,
            IMapper mapper,
            UserService userService,
            UserGamesService userGamesService,
            IPaginationService<UserGame> paginationService)
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _userGamesService = userGamesService;
            _paginationService = paginationService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfileInfo(string userId)
        {
            try
            {
                if (!Guid.TryParse(userId, out var userGuid))
                {
                    _logger.LogWarning("Invalid user ID format");
                    return BadRequest("Invalid user ID format");
                }

                var profile = _mapper.Map<ProfileDTO>(await _userService.GetById(userGuid));
                if (profile == null)
                {
                    _logger.LogWarning("User not found");
                    return NotFound("User not found");
                }

                bool isSelfProfile = userGuid == Guid.Parse(Request.Cookies["userId"]);

                return Ok(new { profile, isSelfProfile });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting profile info");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("{userId}/games")]
        public async Task<IActionResult> GetPaginatedGames(string userId, [FromQuery] int pageNumber,[FromQuery]int pageSize)
        {
            if (!Guid.TryParse(userId, out var userGuid))
                return BadRequest("Invalid user ID");

            var query = await _userGamesService.GetAllUserGames(userGuid);
            (IEnumerable<UserGame?> games, int totalCount) = _paginationService.Paginate(query, pageNumber - 1, pageSize);

            var userGamesDTO = await _userGamesService.MapUserGames(games);

            return Ok(new
            {
                userGamesDTO,
                totalCount,
            });
        }

        //public class ProfileResponse
        //{
        //    public ProfileDTO Profile { get; set; }
        //    public bool IsSelfProfile { get; set; }
        //    public IEnumerable<ProfileGameDTO> UserGames { get; set; }
        //    public int TotalCount { get; set; }
        //}
    }
}