using Data_Access.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Data_Access.Dto_Models;

[ApiController]
[Route("api/admin")]
public class AdminGameController : ControllerBase
{
    private readonly IListRepository<Game> _gameRepo;
    private readonly IRepository<GameGallery> _galleryRepo;
    private readonly IHostEnvironment _env;

    public AdminGameController(
        IListRepository<Game> gameRepo,
        IRepository<GameGallery> galleryRepo,
        IHostEnvironment env)
    {
        _gameRepo = gameRepo;
        _galleryRepo = galleryRepo;
        _env = env;
    }

    [HttpGet("games")]
    public async Task<IActionResult> GetGames()
    {
        var games = await _gameRepo.GetAll();
        return Ok(games);
    }


    [HttpPost("add-game")]
    public async Task<IActionResult> AddGame([FromForm] GameDto request)
    {
        try
        {
            if (request.LogoFile == null || request.LogoFile.Length == 0)
                return BadRequest("Логотип обязателен");

            string logoPath = await SaveFileAsync(request.LogoFile, "logos");

            var game = new Game
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Discount = request.Discount,
                Logo = logoPath,
                Developer = request.Developer,
                RecommendedRequirementId = request.RecommendedRequirementId,
                MinimumRequirementId = request.MinimumRequirementId,
                ReleaseDate = request.ReleaseDate
            };

            await _gameRepo.Add(game);
            if (request.GalleryFiles != null && request.GalleryFiles.Count > 0)
            {
                foreach (var file in request.GalleryFiles)
                {
                    string filePath = await SaveFileAsync(file, "gallery");
                    await _galleryRepo.Add(new GameGallery
                    {
                        Id = Guid.NewGuid(),
                        Link = filePath,
                        IsCover = false,
                        Type = file.ContentType.StartsWith("image") ? "photo" : "video",
                        GameId = game.Id
                    });
                }
            }

            return Ok(new { Success = true, GameId = game.Id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    private async Task<string> SaveFileAsync(IFormFile file, string subfolder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Файл не может быть пустым");

        // Создаем папку если не существует
        string uploadsDir = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", subfolder);
        Directory.CreateDirectory(uploadsDir);

        // Генерируем уникальное имя файла
        string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        string filePath = Path.Combine(uploadsDir, uniqueFileName);

        // Сохраняем файл
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return $"/uploads/{subfolder}/{uniqueFileName}";
    }
}