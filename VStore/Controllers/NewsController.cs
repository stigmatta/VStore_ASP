using Business_Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace VStore.Controllers
{
    [Route("api/[controller]")]
    public class NewsController:Controller
    {
        private readonly NewsService _newsService;
        public NewsController(NewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allNews = await _newsService.GetAll();
            var bigNews = allNews.Take(2);
            var smallNews = allNews.Skip(2);
            return Ok(new
                { bigNews, smallNews}
            );
        }
    }
}
