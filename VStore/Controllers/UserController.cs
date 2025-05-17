using Business_Logic.Services;
using Microsoft.EntityFrameworkCore;
using Data_Transfer_Object.DTO.UserDTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Data_Transfer_Object.DTO.User;
using Data_Access.Models;

namespace VStore.Controllers
{
    [Route("api/")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly IWebHostEnvironment _environment;

        public UserController(
            IMapper mapper,
            UserService userService,
            ILogger<UserController> logger,
            IWebHostEnvironment environment)
        {
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
            _environment = environment;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var isAuthorized = Request.Cookies.ContainsKey("username");
            if (isAuthorized == true)
            {
                var userId = Request.Cookies["userId"];
                return Ok(new { isAuthorized, userId });
            }
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
            }
            catch (DbUpdateException)
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
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
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
            Response.Cookies.Delete("username", cookieOptions);
            Response.Cookies.Delete("userId", cookieOptions);
            Response.Cookies.Delete("isAdmin", cookieOptions);
            return Ok("Logged out");
        }
        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser()
        {
            var userId = Request.Cookies["userId"];
            var user = await _userService.GetById(Guid.Parse(userId));
            if (user == null)
            {
                _logger.LogInformation("User not found");
                return NotFound("User not found");
            }
            var userDTO = _mapper.Map<ProfileDTO>(user);
            return Ok(userDTO);
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            if (!Request.Cookies.TryGetValue("userId", out var userId))
            {
                return Unauthorized("User not authenticated");
            }

            if (!Guid.TryParse(userId, out var userGuid))
            {
                return BadRequest("Invalid user ID format");
            }
            try
            {
                var users = await _userService.GetAll();
                var userDtos = _mapper.Map<List<ProfileDTO>>(users);
                var withoutSelf = userDtos.Where(x => x.Id != userGuid).ToList();

                return Ok(withoutSelf);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                return StatusCode(500, "An error occurred while fetching users");
            }
        }
        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileRequest request)
        {
            if (!Request.Cookies.TryGetValue("userId", out var userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            try
            {
                var user = await _userService.GetById(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                if (request.Avatar != null && request.Avatar.Length > 0)
                {
                    var avatarUrl = await SaveAvatar(request.Avatar);
                    if (!string.IsNullOrEmpty(user.Photo) && !user.Photo.StartsWith("http"))
                    {
                        var fullPath = Path.Combine(_environment.WebRootPath, user.Photo.TrimStart('/'));
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                            _logger.LogInformation("Deleted old avatar: {Path}", fullPath);
                        }
                    }
                    await _userService.UpdateAvatar(userId, avatarUrl);
                    user.Photo = avatarUrl;
                }

                if (!string.IsNullOrWhiteSpace(request.Username) && request.Username != user.Username)
                {
                    if (await _userService.CheckUsernameExists(request.Username))
                    {
                        return BadRequest(new { message = "Username already taken" });
                    }

                    await _userService.UpdateUsername(userId, request.Username);
                    user.Username = request.Username;

                    Response.Cookies.Append("username", user.Username, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTimeOffset.Now.AddHours(2)
                    });
                }

                if (!string.IsNullOrWhiteSpace(request.OldPassword) && !string.IsNullOrWhiteSpace(request.NewPassword))
                {
                    var verifiedUser = await _userService.VerifyUser(user.Username, request.OldPassword);
                    if (verifiedUser == null)
                    {
                        return BadRequest(new { message = "Old password is incorrect" });
                    }

                    await _userService.UpdatePassword(userId, request.NewPassword);
                }

                var userDto = _mapper.Map<ProfileDTO>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile");
                return StatusCode(500, new { message = "An error occurred while updating profile" });
            }
        }

        private async Task<string> SaveAvatar(IFormFile avatarFile)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif",".webp" };
            var fileExtension = Path.GetExtension(avatarFile.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Invalid file type");
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "avatars");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatarFile.CopyToAsync(stream);
            }

            return $"/avatars/{fileName}";
        }


        public class UpdateProfileRequest
        {
            public string Username { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
            public IFormFile Avatar { get; set; }
        }
    }
    }
