using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "CookieUserPolicy")]
    public class CartController: Controller
    {
        private readonly ILogger<UserController> _logger;
        public CartController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Cart redirection");
        }
    }
}
