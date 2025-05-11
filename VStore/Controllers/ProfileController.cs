using AutoMapper;
using Business_Logic.Services;
using Data_Transfer_Object.DTO.Game;
using Data_Transfer_Object.DTO.GameDTO;
using Data_Transfer_Object.DTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VStore.Controllers
{
    [Authorize(Policy = "CookieUserPolicy")]
    [Route("api/[controller]")]
    public class ProfileController: ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly UserGamesService _userGamesService;
        public ProfileController(ILogger<UserController> logger,IMapper mapper,UserService userService,UserGamesService userGamesService)
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _userGamesService = userGamesService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfileInfo(string userId)
        {
            var userGuid = Guid.Parse(userId);
            bool isSelfProfile = false;
            if (userGuid == Guid.Empty)
            {
                _logger.LogWarning("Invalid user ID");
                return BadRequest("Invalid user ID");
            }
            var profile = _mapper.Map<ProfileDTO>(await _userService.GetById(userGuid));
            if (profile == null)
            {
                _logger.LogWarning("User not found");
                return NotFound("User not found");
            }
            if(userGuid == Guid.Parse(Request.Cookies["userId"]))
                isSelfProfile = true;
            var userGames = await _userGamesService.GetAllUserGames(userGuid);
            var userGamesDTO = await _userGamesService.MapUserGames(userGames);
            var profileResponse = new ProfileResponse
            {
                Profile = profile,
                IsSelfProfile = isSelfProfile,
                UserGames = userGamesDTO
            };
            return Ok(profileResponse);
        }

        public class ProfileResponse()
        {
            public ProfileDTO Profile { get; set; }
            public bool IsSelfProfile { get; set; }
            public IEnumerable<ProfileGameDTO> UserGames { get; set; }
        }

    }
}
