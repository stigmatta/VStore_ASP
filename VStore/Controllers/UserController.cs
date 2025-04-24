using AutoMapper;
using Data_Access.Models;
using Microsoft.AspNetCore.Mvc;
using VStore.DTO.User;
using Business_Logic.Services;
using Microsoft.EntityFrameworkCore;

namespace VStore.Controllers
{
    [Route("api/")]
    public class UserController : Controller
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
    }
}
