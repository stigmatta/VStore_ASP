using AutoMapper;
using Business_Logic.Services;
using Data_Transfer_Object.DTO.Game;
using Microsoft.AspNetCore.Mvc;

namespace VStore.Controllers
{
    [Route("api/[controller]")]
    public class MainController:ControllerBase
    {
        private readonly ILogger<MainController> _logger;
        private readonly GameService _gameService;
        private readonly GameGalleryService _gameGalleryService;
        private readonly IMapper _mapper;
        public MainController(ILogger<MainController> logger,GameService gameService, GameGalleryService gameGalleryService,IMapper mapper)
        {
            _logger = logger;
            _gameService = gameService;
            _gameGalleryService = gameGalleryService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Main page visited");

            var mainGame = await _gameService.GetGameByName("Dead by Daylight");
            var mainGameGallery = await _gameGalleryService.GetByGameId(mainGame.Id);
            var mainGameWithGallery = _gameService.ConnectGameWithGallery(mainGame, mainGameGallery);
            await _gameService.GetMostPlayedWithHours();

            var response = new MainPageResponse
            {
                MainGameWithGallery = mainGameWithGallery,
                DiscoverNew = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetRecentGames()),
                WithDiscount = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetOnSale()),
                DealOfTheWeek = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetDealOfTheWeek()),
                FreeGames = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetFreeGames()),
                PopularGames = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetPopularGames()),
                MostPlayed = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetMostPlayed()),
                WishlistGames = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetWishlistGames()),
                TopSellers = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetTopSellers()),
                Under5Games = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetUnder5Dollars()),
                Upcoming = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetUpcoming())
            };

            return Ok(response);
        }


        public class MainPageResponse
        {
            public GameDTO MainGameWithGallery { get; set; }
            public List<MainPageGameDTO> DiscoverNew { get; set; }
            public List<MainPageGameDTO> WithDiscount { get; set; }
            public List<MainPageGameDTO> DealOfTheWeek { get; set; }
            public List<MainPageGameDTO> FreeGames { get; set; }
            public List<MainPageGameDTO> PopularGames { get; set; }
            public List<MainPageGameDTO> MostPlayed { get; set; }
            public List<MainPageGameDTO> WishlistGames { get; set; }
            public List<MainPageGameDTO> TopSellers { get; set; }
            public List<MainPageGameDTO> Under5Games { get; set; }
            public List<MainPageGameDTO> Upcoming { get; set; }
        }

    }
}
