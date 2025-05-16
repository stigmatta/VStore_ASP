using AutoMapper;
using Business_Logic.Services;
using Data_Access.Models;
using Data_Transfer_Object.DTO.Achievement;
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
        private readonly UserAchievementService _userAchievementService;
        private readonly RelationshipService _relationshipService;
        private readonly IPaginationService<UserGame> _paginationService;
        private readonly IPaginationService<Achievement> _achievementPaginationService;


        public ProfileController(
            ILogger<ProfileController> logger,
            IMapper mapper,
            UserService userService,
            UserGamesService userGamesService,
            UserAchievementService userAchievementService,
            RelationshipService relationshipService,
            IPaginationService<UserGame> paginationService,
            IPaginationService<Achievement> achievementPaginationService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _userGamesService = userGamesService;
            _userAchievementService = userAchievementService;
            _relationshipService = relationshipService;
            _paginationService = paginationService;
            _achievementPaginationService = achievementPaginationService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(true);
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

                string status;
                var selfUser = Guid.Parse(Request.Cookies["userId"]);
                if (userGuid == selfUser)
                {
                    status = "Self";
                }
                else
                {
                    status = await _relationshipService.GetStatus(userGuid, selfUser);
                }


                var friends = await _relationshipService.GetFriends(userGuid);
                var friendCount = friends?.Count() ?? 0;


                return Ok(new { profile, status, friendCount });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting profile info");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("{userId}/games")]
        public async Task<IActionResult> GetPaginatedGames(string userId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
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

        [HttpGet("{userId}/achievements")]
        public async Task<IActionResult> GetPaginatedAchievements(string userId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            if (!Guid.TryParse(userId, out var userGuid))
                return BadRequest("Invalid user ID");
            var achievements = await _userAchievementService.GetAllUserAchievements(userGuid);
            (IEnumerable<Achievement?> userAchievements, int totalCount) = _achievementPaginationService.Paginate(achievements, pageNumber - 1, pageSize);
            var items = _mapper.Map<IEnumerable<AchievementDTO>>(userAchievements);
            return Ok(new
            {
                items,
                totalCount,
            });
        }

        [HttpGet("{userId}/friends")]
        public async Task<IActionResult> GetFriends([FromRoute] string userId)
        {
            if (!Guid.TryParse(userId, out var userGuid))
                return BadRequest("Invalid user ID format");
            _logger.LogWarning($"USER GUID {userGuid}");

            try
            {
                var friends = await _relationshipService.GetFriends(userGuid);
                if (friends == null || !friends.Any())
                    return Ok(Enumerable.Empty<ProfileDTO>());

                var items = _mapper.Map<IEnumerable<ProfileDTO>>(friends);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching friends");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("{userId}/blocked")]
        public async Task<IActionResult> GetBlocked([FromRoute] string userId)
        {
            if (!Guid.TryParse(userId, out var userGuid))
                return BadRequest("Invalid user ID format");
            _logger.LogWarning($"USER GUID {userGuid}");

            try
            {
                var blocked = await _relationshipService.GetBlocked(userGuid);
                if (blocked == null || !blocked.Any())
                    return Ok(Enumerable.Empty<ProfileDTO>());

                var items = _mapper.Map<IEnumerable<ProfileDTO>>(blocked);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching friends");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("{userId}/pending")]
        public async Task<IActionResult> GetPending([FromRoute] string userId)
        {
            if (!Guid.TryParse(userId, out var userGuid))
                return BadRequest("Invalid user ID format");
            _logger.LogWarning($"USER GUID {userGuid}");

            try
            {
                var pending = await _relationshipService.GetPending(userGuid);
                if (pending == null || !pending.Any())
                    return Ok(Enumerable.Empty<ProfileDTO>());

                var items = _mapper.Map<IEnumerable<ProfileDTO>>(pending);
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching friends");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{userId}/delete/{friendId}")]
        public async Task<IActionResult> DeleteFriend([FromRoute] string userId, [FromRoute] string friendId)
        {
            if (!Guid.TryParse(userId, out var userGuid))
                return BadRequest("Invalid user ID format");
            if (!Guid.TryParse(friendId, out var friendGuid))
                return BadRequest("Invalid friend ID format");
            try
            {
                await _relationshipService.DeleteFriend(userGuid, friendGuid);
                return Ok("Friend deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting friend");
                return StatusCode(500, "Internal server error");
            }


        }
    }
}