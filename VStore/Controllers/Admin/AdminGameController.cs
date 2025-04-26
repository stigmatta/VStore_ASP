using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Business_Logic.Services;
using VStore.DTO.Game;
using Microsoft.AspNetCore.Hosting.Server;

[ApiController]
[Route("api/admin/games")]
[Authorize(Policy = "CookieAdminPolicy")]

public class AdminGameController : ControllerBase
{

    private readonly GameService _gameService;
    private readonly GameGalleryService _gameGalleryService;
    private readonly IWebHostEnvironment _env;
    public AdminGameController(GameService gameService, GameGalleryService gameGalleryService,IWebHostEnvironment env)
    {
        _gameService = gameService;
        _gameGalleryService = gameGalleryService;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> GetGames()
    {
        var games = await _gameService.GetAllGames();
        return Ok(games);
    }


    [HttpPost("add-game")]
    public async Task<IActionResult> AddGame([FromForm] GameDTO request)
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
                LogoPath = logoPath,
                Developer = request.Developer,
                RecommendedRequirementId = request.RecommendedRequirementId,
                MinimumRequirementId = request.MinimumRequirementId,
                ReleaseDate = request.ReleaseDate
            };

            await _gameService.AddGame(game);

            if (request.GalleryFiles != null && request.GalleryFiles.Count > 0)
            {
                foreach (var file in request.GalleryFiles)
                {
                    string filePath = await SaveFileAsync(file, "gallery");
                    await _gameGalleryService.AddGameGallery(new GameGallery
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
}