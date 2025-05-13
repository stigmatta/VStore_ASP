using AutoMapper;
using Business_Logic.Services;
using Data_Access.Models;
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
        private readonly IPaginationService<Wishlist> _paginationService;
        public WishlistController(ILogger<WishlistController> logger, WishlistService wishlistService, IMapper mapper, IPaginationService<Wishlist> pagination)
        {
            _logger = logger;
            _wishlistService = wishlistService;
            _mapper = mapper;
            _paginationService = pagination;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int pageNumber, [FromQuery]int pageSize, [FromQuery] string sortOption)
        {
            var userId = Request.Cookies["userId"];
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is missing");
            List<Wishlist> wishlist = new List<Wishlist>();
            if (string.IsNullOrEmpty(sortOption) || sortOption == "none")
                wishlist = await _wishlistService.GetUserWishlist(Guid.Parse(userId));
            else if (sortOption == "sale")
                wishlist = await _wishlistService.GetUserOnSale(Guid.Parse(userId));
            (IEnumerable<Wishlist?> wishlists, int totalCount) = _paginationService.Paginate(wishlist, pageNumber - 1, pageSize);
            var games = _mapper.Map<IEnumerable<MainPageGameDTO>>(wishlists.Select(w => w.Game).ToList());

            return Ok(new { games, totalCount });
        }

        [HttpPost("check-game")]
        public async Task<IActionResult> CheckIfExists([FromBody] Guid gameId)
        {
            var userId = Request.Cookies["userId"];
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID is missing");

            var games = await _wishlistService.GetUserWishlist(Guid.Parse(userId));
            var isExists = games.Any(g => g.UserId == Guid.Parse(userId) && g.GameId == gameId);

            if (isExists)
                return BadRequest();
            else
                return Ok();
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
