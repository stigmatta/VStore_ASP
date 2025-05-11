using Microsoft.AspNetCore.Mvc;
using Business_Logic.Services;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;
using Data_Transfer_Object.DTO.AdminAchievement;


namespace VStore.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/achievements")]
    public class AdminAchievementController : ControllerBase
    {
        private readonly AchievementService _achiService;
        private readonly GameService _gameService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AdminAchievementController> _logger;

        public AdminAchievementController(
            AchievementService achiService,
            IWebHostEnvironment env,
            ILogger<AdminAchievementController> logger,
            GameService gameService)
        {
            _achiService = achiService;
            _env = env;
            _logger = logger;
            _gameService = gameService;
        }

        [HttpGet("game-{gameId}")]
        public async Task<IActionResult> GetAllAchievements(Guid gameId)
        {
            try
            {
                var achievements = await _achiService.GetAll(gameId);
                return Ok(achievements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all achievements for game {GameId}", gameId);
                return StatusCode(500, new { Error = "Failed to get achievements list", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAchievementById(Guid id)
        {
            try
            {
                var achievement = await _achiService.GetById(id);
                if (achievement == null)
                    return NotFound(new { Error = "Achievement not found" });

                return Ok(achievement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting achievement by ID {AchievementId}", id);
                return StatusCode(500, new { Error = "Error while getting achievement", Details = ex.Message });
            }
        }

        [HttpPost("add-achievement")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<Guid>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        public async Task<IActionResult> AddAchievement([FromForm] AdminAchievementDTO request)
        {
            try
            {
                // 1. Валидация модели
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    _logger.LogWarning("Invalid model state: {Errors}", string.Join(", ", errors));
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Error = "Invalid request data",
                        ValidationErrors = errors
                    });
                }

                // 2. Проверка изображения
                if (request.Photo == null || request.Photo.Length == 0)
                {
                    _logger.LogWarning("Photo file is missing");
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Error = "Photo is required"
                    });
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(request.Photo.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    _logger.LogWarning("Invalid file format: {Extension}", fileExtension);
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Error = "Invalid file format",
                        Details = $"Allowed formats: {string.Join(", ", allowedExtensions)}"
                    });
                }


                _logger.LogInformation("Adding new achievement: {Title}", request.Title);

                string photoPath;
                try
                {
                    photoPath = await SaveFileAsync(request.Photo, "achievements");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving achievement photo");
                    return StatusCode(500, new ApiResponse
                    {
                        Success = false,
                        Error = "Error saving file",
                        Details = ex.Message
                    });
                }
                var achievement = new Achievement
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Description = request.Description,
                    Photo = photoPath,
                    GameId = request.GameId,
                };

                try
                {
                    await _achiService.AddAchievement(achievement);
                }
                catch (DbUpdateException dbEx)
                {
                    _logger.LogError(dbEx, "Database error while adding achievement");

                    await DeleteFileAsync(photoPath);

                    return StatusCode(500, new ApiResponse
                    {
                        Success = false,
                        Error = "Database error",
                        Details = dbEx.InnerException?.Message ?? dbEx.Message
                    });
                }

                _logger.LogInformation("Achievement added successfully. ID: {Id}", achievement.Id);

                return Ok(new ApiResponse<Guid>
                {
                    Success = true,
                    Data = achievement.Id,
                    Message = "Achievement added successfully",
                    Metadata = new
                    {
                        ImageUrl = $"{Request.Scheme}://{Request.Host}/{photoPath.TrimStart('/')}"
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error adding achievement");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Error = "Internal server error",
                    Details = ex.Message
                });
            }
        }

        public class ApiResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public string Error { get; set; }
            public string Details { get; set; }
            public IEnumerable<string> ValidationErrors { get; set; }
        }

        public class ApiResponse<T> : ApiResponse
        {
            public T Data { get; set; }
            public object Metadata { get; set; }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAchievement(Guid id)
        {
            try
            {
                var achievement = await _achiService.GetById(id);
                if (achievement == null)
                    return NotFound(new { Error = "Achievement not found" });

                await DeleteFileAsync(achievement.Photo);
                await _achiService.DeleteAchievement(id);

                return Ok(new
                {
                    Success = true,
                    Message = "Achievement deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting achievement {AchievementId}", id);
                return StatusCode(500, new
                {
                    Error = "Failed to delete achievement",
                    Details = ex.Message
                });
            }
        }

        private async Task<string> SaveFileAsync(IFormFile file, string subfolder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be empty");

            string uploadsDir = Path.Combine(_env.WebRootPath, "uploads", subfolder);
            Directory.CreateDirectory(uploadsDir);

            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(uploadsDir, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{subfolder}/{uniqueFileName}";
        }

        private async Task DeleteFileAsync(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var physicalPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/'));
                if (System.IO.File.Exists(physicalPath))
                {
                    await Task.Run(() => System.IO.File.Delete(physicalPath));
                }
            }
        }
    }
}