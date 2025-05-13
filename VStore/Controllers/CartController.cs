using AutoMapper;
using Business_Logic.Services;
using Data_Access.Models;
using Data_Transfer_Object.DTO.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "CookieUserPolicy")]
    public class CartController: ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly IMapper _mapper;
        private readonly UserGamesService _userGamesService;
        private readonly UserService _userService;
        private readonly UserAchievementService _userAchievementService;

        public CartController(ILogger<CartController> logger,IMapper mapper,UserGamesService userGamesService,UserService userService,UserAchievementService userAchievementService)
        {
            _logger = logger;
            _mapper = mapper;
            _userGamesService = userGamesService;
            _userService = userService;
            _userAchievementService = userAchievementService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Cart redirection");
        }
        [HttpPost("order")]
        public async Task<IActionResult> AddGames([FromBody] OrderRequest request)
        {
            Random rand = new Random();

            if (request == null)
            {
                _logger.LogWarning("Request is null");
                return BadRequest("Request body is required");
            }


            var userId = Request.Cookies["userId"];
            var userGuid = Guid.Parse(userId);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is missing");
            var user = await _userService.GetById(userGuid);
            if (user == null)
                return BadRequest("User not found");
            foreach (var game in request.Games)
            {
                try
                {
                    await _userGamesService.AddUserGame(new UserGame
                    {
                        UserId = userGuid,
                        GameId = game.Id,
                        CompletedPercent = rand.Next(0, 101),
                        HoursPlayed = rand.Next(0, 1000),
                    });
                }
                catch (DbUpdateException)
                {
                    return BadRequest($"{game.Title} already in your library");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding game to cart");
                    return StatusCode(500, "Internal server error");
                }
            }


            if (request.SaveMethod == false)
            {

                Response.Cookies.Append("cardType", "", new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.Now.AddDays(-1),
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
            }
            else
            {
                Response.Cookies.Append(
                    "cardType",
                    request.Selected.ToLower(),
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTimeOffset.Now.AddHours(2),
                    }
                );
            }

            return Ok();
        }

        [HttpGet("savedMethod")]
        public IActionResult GetSavedMethod()
        {
            var saveMethod = Request.Cookies["cardType"];
            if (string.IsNullOrEmpty(saveMethod))
                return BadRequest("Save method is missing");
            return Ok(saveMethod);
        }


        public class OrderRequest
        {
            [JsonPropertyName("games")]
            public List<MainPageGameDTO> Games { get; set; }

            [JsonPropertyName("selected")]  
            public string Selected { get; set; } 

            [JsonPropertyName("saveMethod")] 
            public bool SaveMethod { get; set; } 
        }
    }
}
