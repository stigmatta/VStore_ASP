
using Data_Access.Interfaces;
using Data_Access.Models;

namespace Business_Logic.Services
{
    public class WishlistService
    {
        private readonly IUnitOfWork Database;
        public WishlistService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task<IEnumerable<Wishlist>> GetTopWishlistGames()
        {
            var games = await Database.WishlistRepository.GetAll();

            return games
                .GroupBy(g => g.GameId)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .SelectMany(g => g)
                .ToList();
        }
    }
}
