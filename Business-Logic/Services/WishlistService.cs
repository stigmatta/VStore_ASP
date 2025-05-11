
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Business_Logic.Services
{
    public class WishlistService
    {
        private readonly IUnitOfWork Database;
        private readonly ILogger<WishlistService> _logger;
        public WishlistService(IUnitOfWork uow, ILogger<WishlistService> logger)
        {
            _logger = logger;
            Database = uow;
        }
        public async Task<IEnumerable<Wishlist?>> GetTopWishlistGames()
        {
            var games = await Database.WishlistRepository.GetAll();

            return games
                .GroupBy(g => g.GameId)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .SelectMany(g => g)
                .ToList();
        }

        public async Task<List<Wishlist>> GetUserWishlist(Guid userId)
        {
            var result = await Database.WishlistRepository.GetByUserId(userId);
            return result;
        }

        public async Task AddToWishlist(Guid userId, Guid gameId)
        {
            try
            {
                await Database.WishlistRepository.Add(userId,gameId);
                await Database.Save();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<List<Wishlist>> GetUserOnSale(Guid userId)
        {
            var result = await Database.WishlistRepository.GetByUserId(userId);
            var withDiscount = result.Where(g => g.Game.Discount != null && g.Game.Discount > 0).ToList();
            return withDiscount;
        }
        public async Task RemoveFromWishlist(Guid userId,Guid gameId)
        {
            await Database.WishlistRepository.Delete(userId,gameId);
            await Database.Save();
        }
    }
}
