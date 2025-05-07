using AutoMapper;
using Data_Access.Models;
using Microsoft.AspNetCore.Mvc;
using Data_Transfer_Object.DTO.User;
using Business_Logic.Services;
using Microsoft.EntityFrameworkCore;

namespace VStore.Controllers
{
    [Route("api/")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IMapper mapper,UserService userService,ILogger<UserController> logger)
        {
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var isAuthorized = Request.Cookies.ContainsKey("username");
            return Ok(isAuthorized);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO regDTO)
        {
            _logger.LogInformation("Registering user with email: {Email}", regDTO.Email);
            if (regDTO == null || string.IsNullOrEmpty(regDTO.Email) || string.IsNullOrEmpty(regDTO.Password))
            {
                _logger.LogInformation("Invalid input");
                return BadRequest("Invalid input");
            }
            var user = _mapper.Map<User>(regDTO);
            try
            {
                await _userService.Create(user);
            }catch(DbUpdateException)
            {
                _logger.LogInformation("DbUpdateException");
                return BadRequest("User with this email already exists");
            }
            catch (Exception)
            {
                _logger.LogInformation("Internal error");
                return StatusCode(500, "Internal server error");
            }
            _logger.LogInformation("User registered successfully");
            return Ok("User registered successfully");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDTO loginDTO)
        {
            _logger.LogInformation("Logging in user with email: {username}", loginDTO.Username);
            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Username) || string.IsNullOrEmpty(loginDTO.Password))
            {
                _logger.LogInformation("Invalid input");
                return BadRequest("Invalid input");
            }

            var user = await _userService.VerifyUser(loginDTO.Username, loginDTO.Password);

            if (user == null)
            {
                _logger.LogInformation("Invalid credentials");
                return BadRequest("Invalid credentials");
            }
            if (Request.Cookies.Count != 0)
            {
                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
            }


            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.Now.AddHours(2)
            };
            Response.Cookies.Append("isAdmin", user.IsAdmin.ToString(), cookieOptions);
            Response.Cookies.Append("username", user.Username, cookieOptions);
            Response.Cookies.Append("userId", user.Id.ToString(), cookieOptions);

            return Ok("Login successful");
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            _logger.LogInformation("Logging out...");
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.Now.AddHours(2)
            };
            Response.Cookies.Delete("username",cookieOptions);
            Response.Cookies.Delete("userId", cookieOptions);
            Response.Cookies.Delete("isAdmin", cookieOptions);
            return Ok("Logged out");
        }
    }
}
