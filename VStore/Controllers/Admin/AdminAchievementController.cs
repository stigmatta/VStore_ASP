using Microsoft.AspNetCore.Mvc;
using Business_Logic.Services;
using Data_Transfer_Object.DTO;
using Data_Access.Models;


namespace VStore.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/achievements")]
    public class AdminAchievementController : ControllerBase
    {
        private readonly AchievementService _achiService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AdminAchievementController> _logger;

        public AdminAchievementController(
            AchievementService achiService,
            IWebHostEnvironment env,
            ILogger<AdminAchievementController> logger)
        {
            _achiService = achiService;
            _env = env;
            _logger = logger;
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

        [HttpPost]
        public async Task<IActionResult> AddAchievement([FromForm] AchievementDTO request)
        {
            try
            {
                if (request.Photo == null || request.Photo.Length == 0)
                    return BadRequest("Photo is required");

                _logger.LogInformation("Adding new achievement: {AchievementTitle}", request.Title);

                string photoPath = await SaveFileAsync(request.Photo, "achievements");

                var achievement = new Achievement
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Description = request.Description,
                    Photo = photoPath,
                    GameId = request.GameId
                };

                await _achiService.AddAchievement(achievement);

                _logger.LogInformation("Achievement added successfully: {AchievementTitle} (ID: {AchievementId})",
                    achievement.Title, achievement.Id);

                return Ok(new
                {
                    Success = true,
                    AchievementId = achievement.Id,
                    Message = "Achievement added successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new achievement");
                return StatusCode(500, new
                {
                    Error = "Failed to add achievement",
                    Details = ex.Message
                });
            }
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