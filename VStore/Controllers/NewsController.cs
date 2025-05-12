using AutoMapper;
using Business_Logic.Services;
using Data_Access.Models;
using Data_Transfer_Object.DTO.NewsDTO;
using Microsoft.AspNetCore.Mvc;

namespace VStore.Controllers
{
    [Route("api/[controller]")]
    public class NewsController: ControllerBase
    {
        private readonly NewsService _newsService;
        private readonly IPaginationService<News> _paginationService;
        private readonly ILogger<NewsController> _logger;
        private readonly IMapper _mapper;
        public NewsController(NewsService newsService, IPaginationService<News> paginationService, ILogger<NewsController> logger, IMapper mapper)
        {
            _newsService = newsService;
            _paginationService = paginationService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var allNews = await _newsService.GetAll();
            (IEnumerable<News?> news, int totalCount) = _paginationService.Paginate(allNews, pageNumber - 1, pageSize);
            var newsDTO = _mapper.Map<IList<NewsDTO>>(news);

            var bigNews = newsDTO.Take(2);
            var smallNews = newsDTO.Skip(2);
            return Ok(new { bigNews, smallNews,totalCount });
        }
    }
}
