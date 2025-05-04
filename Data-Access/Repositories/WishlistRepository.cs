using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class WishlistRepository:IListRepository<Wishlist>
    {
        private readonly StoreContext _context;
        public WishlistRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(Wishlist entity)
        {
            await _context.Wishlists.AddAsync(entity);
        }
        public async Task Delete(Guid id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist != null)
                _context.Wishlists.Remove(wishlist);
        }
        public async Task<Wishlist?> GetById(Guid? id)
        {
            return await _context.Wishlists.FindAsync(id);
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
    }
}
