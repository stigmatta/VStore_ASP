using Microsoft.AspNetCore.Mvc;
using Business_Logic.Services;
using Data_Access.Models;
using Data_Transfer_Object.DTO;

namespace VStore.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/news")]
    public class AdminNewsController : ControllerBase
    {
        private readonly NewsService _newsService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AdminNewsController> _logger;

        public AdminNewsController(NewsService newsService, IWebHostEnvironment env, ILogger<AdminNewsController> logger)
        {
            _newsService = newsService;
            _env = env;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            try
            {
                var news = await _newsService.GetAll();
                return Ok(news);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Не удалось получить список новостей", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(Guid id)
        {
            try
            {
                var news = await _newsService.GetById(id);
                if (news == null)
                    return NotFound(new { Error = "Новость не найдена" });

                return Ok(news);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Ошибка при получении новости", Details = ex.Message });
            }
        }

        [HttpPost("add-news")]
        public async Task<IActionResult> AddNews([FromForm] NewsDTO request)
        {
            try
            {
                if (request.PhotoFile == null || request.PhotoFile.Length == 0)
                    return BadRequest("Фотография обязательна");

                _logger.LogInformation($"Добавляетс новость:");

                string photoPath = await SaveFileAsync(request.PhotoFile, "news");

                var news = new News
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Photo = photoPath,
                    PublishedDate = request.PublishedDate
                };

                await _newsService.AddNews(news);
                _logger.LogInformation($"Добавлена новость: {news.Title}");
                return Ok(new { Success = true, NewsId = news.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Не удалось добавить новость", Details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(Guid id)
        {
            try
            {
                var news = await _newsService.GetById(id);
                if (news == null)
                    return NotFound(new { Error = "Новость не найдена" });

                await DeleteNewsFiles(news);
                await _newsService.DeleteNews(id);

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Не удалось удалить новость", Details = ex.Message });
            }
        }

        private async Task<string> SaveFileAsync(IFormFile file, string subfolder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Файл не может быть пустым");

            string uploadsDir = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", subfolder);
            Directory.CreateDirectory(uploadsDir);

            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(uploadsDir, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{subfolder}/{uniqueFileName}";
        }

        private async Task DeleteNewsFiles(News news)
        {
            if (!string.IsNullOrEmpty(news.Photo))
            {
                var photoPath = Path.Combine(_env.ContentRootPath, "wwwroot", news.Photo.TrimStart('/'));
                if (System.IO.File.Exists(photoPath))
                {
                    System.IO.File.Delete(photoPath);
                }
            }
        }
    }
}