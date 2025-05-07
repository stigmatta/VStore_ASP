using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VStore.Controllers
{
    [Authorize(Policy = "CookieUserPolicy")]
    [Route("api/[controller]")]
    public class ProfileController: ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        public ProfileController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            
            return Ok("Profile redirection");
        }
    }
}
