using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class WishlistRepository:IManyToMany<Guid,Guid>
    {
        private readonly StoreContext _context;
        public WishlistRepository(StoreContext context)
        {
            _context = context;
        }
        //public async Task Add(Wishlist entity)
        //{
        //    await _context.Wishlists.AddAsync(entity);
        //}
        //public async Task Delete(Guid id)
        //{
        //    var wishlist = await _context.Wishlists.FindAsync(id);
        //    if (wishlist != null)
        //        _context.Wishlists.Remove(wishlist);
        //}
        public async Task<Wishlist?> GetById(Guid? id)
        {
            return await _context.Wishlists.FindAsync(id);
        }
        public async Task<List<Wishlist>> GetByUserId(Guid userId)
        {
            return await _context.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Game) 
                .ToListAsync();
        }
        public void Update(Wishlist entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public async Task<IList<Wishlist>> GetAll(Guid?  userId)
        {
            var user = await _context.Users
                .Include(u => u.Wishlist)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
                return user.Wishlist.ToList();
            return new List<Wishlist>();
        }

        public async Task<IEnumerable<Wishlist>> GetAll()
        {
            return await _context.Wishlists.ToListAsync();
        }

        public async Task Add(Guid userId, Guid gameId)
        {
            var wishlist = new Wishlist
            {
                UserId = userId,
                GameId = gameId
            };
            await _context.Wishlists.AddAsync(wishlist);
        }

        public async Task Delete(Guid userId, Guid gameId)
        {
            var wishlist = _context.Wishlists
                .FirstOrDefault(w => w.UserId == userId && w.GameId == gameId);
            if (wishlist != null)
            {
                _context.Wishlists.Remove(wishlist);
            }
            await Task.CompletedTask;

        }
    }
}
