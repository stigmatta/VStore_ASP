using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class ReviewRepository : IRepository<Review>
    {
        private readonly StoreContext _context;
        public ReviewRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(Review entity)
        {
            await _context.Reviews.AddAsync(entity);
        }
        public async Task Delete(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
                _context.Reviews.Remove(review);
        }
        public async Task<Review?> GetById(Guid id)
        {
            return await _context.Reviews.FindAsync(id);
        }
        public void Update(Review entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public async Task<IList<Review>> GetAll(Guid gameId)
        {
            var game = await _context.Games
                .Include(g => g.Reviews)
                .FirstOrDefaultAsync(g => g.Id == gameId);
            if (game != null)
                return game.Reviews.ToList();
            return new List<Review>();
        }
    }
}
