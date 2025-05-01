using AutoMapper;
using Business_Logic.Services;
using Data_Transfer_Object.DTO.Game;
using Microsoft.AspNetCore.Mvc;

namespace VStore.Controllers
{
    [Route("api/[controller]")]
    public class MainController:Controller
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
            var discoverNew = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetRecentGames());
            var withDiscount = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetOnSale());
            var dealOfTheWeek = _mapper.Map <List<MainPageGameDTO>>(await _gameService.GetDealOfTheWeek());
            var freeGames = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetFreeGames());
            var popularGames = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetPopularGames());
            var wishlistGames = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetWishlistGames());
            var topSellers = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetTopSellers());
            var upcoming = _mapper.Map<List<MainPageGameDTO>>(await _gameService.GetUpcoming());
            return Ok(new { mainGameWithGallery, discoverNew,withDiscount,dealOfTheWeek,freeGames,popularGames,wishlistGames,topSellers,upcoming});
        }
    }
}
