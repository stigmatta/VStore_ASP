using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VStore.Controllers.Admin
{
    [Route("api/admin")]
    [Authorize(Policy = "CookieAdminPolicy")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        public AdminController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
