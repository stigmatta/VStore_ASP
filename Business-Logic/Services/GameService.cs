using Data_Access.Interfaces;
using Data_Access.Models;
using Data_Transfer_Object.DTO.Game;
using System.Diagnostics;

namespace Business_Logic.Services
{
    public class GameService
    {
        private readonly IUnitOfWork Database;
        private readonly WishlistService _wishlistService;
        public GameService(IUnitOfWork unitOfWork, WishlistService wishlistService)
        {
            Database = unitOfWork;
            _wishlistService = wishlistService;
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
            var allGames = await Database.GameRepository.GetAll();
            var wishlistGames = await _wishlistService.GetTopWishlistGames();
            return allGames.Where(x => wishlistGames.Any(w => w.GameId == x.Id)).Take(10);
        }
        public async Task<IEnumerable<Game>> GetTopSellers()
        {
            var allGames = await GetReleasedGames();
            var topSellers = allGames.Where(p => p.Price > 0).Take(3);
            return topSellers;
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
