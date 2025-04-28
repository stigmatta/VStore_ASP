using Data_Access.Interfaces;
using Data_Access.Repositories;

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

        public async Task<IEnumerable<Game>> GetRecentGames()
        {
            var allGames = await Database.GameRepository.GetAll();
            var recentGames = allGames.OrderByDescending(x => x.ReleaseDate).Take(10);
            return recentGames;
        }
        public async Task <IEnumerable<Game>> GetOnSale()
        {
            var allGames = await Database.GameRepository.GetAll();
            var onSaleGames = allGames.Where(x => x.Discount > 0).Take(10);
            return onSaleGames;
        }
        public async Task<IEnumerable<Game>> GetDealOfTheWeek()
        {
            var allGames = await Database.GameRepository.GetAll();
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
            var allGames = await Database.GameRepository.GetAll();
            var popularGames = allGames.OrderByDescending(x => x.Reviews.Count).Take(10);
            return popularGames;
        }
        public async Task<IEnumerable<Game>> GetWishlistGames()
        {
            var allGames = await Database.GameRepository.GetAll();
            var wishlistGames = await _wishlistService.GetTopWishlistGames();
            return allGames.Where(x => wishlistGames.Any(w => w.GameId == x.Id)).Take(10);
        }

        //public async Task<IEnumerable<Game>> GetTopRated()
        //{
        //    var allGames = await Database.GameRepository.GetAll();
        //    var topRatedGames = allGames.OrderByDescending(x => x.Reviews.Average(x => x.Rating)).Take(10);
        //    return topRatedGames;
        //}
    }
}
