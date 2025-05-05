using AutoMapper;
using Business_Logic.Services;
using Data_Transfer_Object.DTO.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "CookieUserPolicy")]
    public class WishlistController : ControllerBase
    {
        private readonly ILogger<WishlistController> _logger;
        private readonly WishlistService _wishlistService;
        private readonly IMapper _mapper;
        public WishlistController(ILogger<WishlistController> logger,WishlistService wishlistService,IMapper mapper)
        {
            _logger = logger;
            _wishlistService = wishlistService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Request.Cookies["userId"];
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is missing");
            var wishlist = await _wishlistService.GetUserWishlist(Guid.Parse(userId));
            var games = _mapper.Map<List<MainPageGameDTO>>(wishlist.Select(w => w.Game).ToList());
            return Ok(games);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToWishlist([FromBody] Guid gameId)
        {
            var userId = Request.Cookies["userId"];
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is missing");
            try
            {
                await _wishlistService.AddToWishlist(Guid.Parse(userId), gameId);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding game to wishlist");
                return BadRequest("Game already in wishlist");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding game to wishlist");
                return StatusCode(500, "Internal server error");
            }
            return Ok("Game added to wishlist");
        }

        [HttpDelete("{gameId}")]
        public async Task<IActionResult> RemoveFromWishlist(Guid gameId)
        {
            var userId = Request.Cookies["userId"];
            try
            {
                await _wishlistService.RemoveFromWishlist(Guid.Parse(userId), gameId);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error removing game from wishlist");
                return BadRequest("Game not found in wishlist");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing game from wishlist");
                return StatusCode(500, "Internal server error");
            }
            return Ok("Game removed from wishlist");
        }

    }
}
