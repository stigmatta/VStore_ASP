using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "CookieUserPolicy")]
    public class WishlistController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Wishlist redirection");
        }
    }
}
