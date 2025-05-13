using Data_Access.Interfaces;
using Data_Access.Models;
using Data_Transfer_Object.DTO.Game;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Business_Logic.Services
{
    public class GameService
    {
        private readonly IUnitOfWork Database;
        private readonly WishlistService _wishlistService;
        private readonly ILogger<GameService> _logger;
        public GameService(IUnitOfWork unitOfWork, WishlistService wishlistService,ILogger<GameService> logger)
        {
            Database = unitOfWork;
            _wishlistService = wishlistService;
            _logger = logger;
        }

        public async Task<MinimumRequirement> GetMinimumRequirement(Guid gameId)
        {
            var game = await Database.GameRepository.GetById(gameId);
            var minimumRequirement = await Database.MinimumRequirementRepository.GetById(game.MinimumRequirementId);
            return minimumRequirement;
        }
        public async Task<RecommendedRequirement> GetRecommendedRequirement(Guid gameId)
        {
            var game = await Database.GameRepository.GetById(gameId);
            var recommendedRequirement = await Database.RecommendedRequirementRepository.GetById(game.RecommendedRequirementId);
            return recommendedRequirement;
        }

        public async Task<IEnumerable<Game>> GetAllGames()
        {
            return await Database.GameRepository.GetAll();
        }

        public async Task AddGame(Game game)
        {
            await Database.GameRepository.Add(game);
            await Database.Save();
        }
        public async Task DeleteGame(Guid id)
        {
            await Database.GameRepository.Delete(id);
            await Database.Save();
        }
        public async Task<Game> GetById(Guid id)
        {
            Game game = await Database.GameRepository.GetById(id);
            return game;
        }

        public async Task<IEnumerable<Game>> GetReleasedGames()
        {
            var allGames = await Database.GameRepository.GetAll();
            var today = DateOnly.FromDateTime(DateTime.Now);
            return allGames.Where(x => x.ReleaseDate <= today);
        }
        public async Task<IEnumerable<Game>> GetUnreleasedGames()
        {
            var allGames = await Database.GameRepository.GetAll();
            var today = DateOnly.FromDateTime(DateTime.Now);
            return allGames.Where(x => x.ReleaseDate > today);
        }


        public async Task<IEnumerable<Game>> GetRecentGames()
        {
            var allGames = await GetReleasedGames();
            var recentGames = allGames.OrderByDescending(x => x.ReleaseDate).Take(10);
            return recentGames;
        }
        public async Task <IEnumerable<Game>> GetOnSale()
        {
            var allGames = await GetReleasedGames();
            var onSaleGames = allGames.Where(x => x.Discount > 0).Take(10);
            return onSaleGames;
        }
        public async Task<IEnumerable<Game>> GetDealOfTheWeek()
        {
            var allGames = await GetReleasedGames();
            var dealOfTheWeek = allGames.OrderByDescending(x => x.Discount).Take(3);
            return dealOfTheWeek;
        }
        public async Task<IEnumerable<Game>> GetFreeGames()
        {
            var allGames = await Database.GameRepository.GetAll();
            var freeGames = allGames.Where(x => x.Price == 0).Take(10);
            return freeGames;
        }
        public async Task<IEnumerable<Game>> GetPopularGames()
        {
            var allGames = await GetReleasedGames();
            var popularGames = allGames.OrderByDescending(x => x.Reviews.Count).Take(10);
            return popularGames;
        }
        public async Task<IEnumerable<Game>> GetWishlistGames()
        {
            var allGames = await GetUnreleasedGames();
            var wishlistGames = await _wishlistService.GetTopWishlistGames();
            return allGames.Where(x => wishlistGames.Any(w => w.GameId == x.Id)).Take(3);
        }
        public async Task<IEnumerable<Game>> GetTopSellers()
        {
            var allGames = await Database.UserGameRepository.GetAll();
            var filteredGames = allGames
                .Where(ug => ug.Game.Price > 0)
                .Select(ug => ug.Game)
                .Distinct()
                .ToList();
            var purchasedGames = filteredGames
                .GroupBy(g => g.Id)
                .OrderByDescending(g => g.Count())  
                .Select(g => g.First())
                .Take(3)
                .ToList();
            return purchasedGames;
        }
        public async Task<IEnumerable<Game>> GetMostPlayed(int topN = 3)
        {
            var allUserGames = await Database.UserGameRepository.GetAll();

            var mostPlayedGames = allUserGames
                .Where(ug => ug.HoursPlayed > 0)
                .GroupBy(ug => ug.GameId)
                .Select(g => new
                {
                    Game = g.First().Game, 
                    TotalHours = g.Sum(ug => ug.HoursPlayed)
                })
                .OrderByDescending(g => g.TotalHours)
                .Take(topN)
                .Select(g => g.Game)
                .ToList();

            return mostPlayedGames;
        }



        public async Task GetMostPlayedWithHours()
        {
            var allUserGames = await Database.UserGameRepository.GetAll();

            var mostPlayedGames = allUserGames
                .Where(ug => ug.HoursPlayed > 0)
                .GroupBy(ug => ug.Game)
                .Select(g => new
                {
                    Game = g.Key,
                    TotalHours = g.Sum(ug => ug.HoursPlayed)
                })
                .OrderByDescending(g => g.TotalHours)
                .Take(3)
                .Select(g => (g.Game, g.TotalHours)) 
                .ToList();

            foreach (var (game, hours) in mostPlayedGames)
            {
                _logger.LogInformation($"{game.Title}: total hours {hours}");
            }

        }
        public async Task<Game?> GetGameByName(string title)
        {
            var allGames = await Database.GameRepository.GetAll();
            var game = allGames.FirstOrDefault(x => x.Title == title); 
            return game;
        }

        public async Task<IEnumerable<Game>> GetUpcoming()
        {
            var allGames = await Database.GameRepository.GetAll();
            var upcoming = allGames.Where(x => x.ReleaseDate > DateOnly.FromDateTime(DateTime.Now)).Take(10);
            return upcoming;
        }
        public async Task<IEnumerable<Game>> GetUnder5Dollars()
        {
            var allGames = await GetReleasedGames();
            var under5Games = allGames.Where(x => x.Price>0 && (x.Price <= 5 || (x.Price - (x.Price * x.Discount / 100)) <= 5));
            Console.WriteLine(under5Games.First().Title);
            return under5Games;
        }

        public async Task<IEnumerable<Game>> GetSearchedGames(string substring)
        {
            var allGames = await Database.GameRepository.GetAll();
            var searchedGames = allGames.Where(x => x.Title.ToLower().Contains(substring.ToLower())).Take(5);
            return searchedGames;
        }

        public GameDTO ConnectGameWithGallery(Game game,IList<GameGallery> gameGallery)
        {
            return new GameDTO
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                Discount = game.Discount,
                LogoLink = game.Logo,
                Developer = game.Developer,
                Publisher = game.Publisher,
                PEGI = game.PEGI,
                TrailerLink = game.TrailerLink,
                RecommendedRequirementId = game.RecommendedRequirementId,
                MinimumRequirementId = game.MinimumRequirementId,
                ReleaseDate = game.ReleaseDate,
                Gallery = gameGallery.Select(g => g.Link).ToList()
            };
        }
    }
}
