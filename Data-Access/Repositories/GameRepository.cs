using Data_Access.Context;
using Data_Access.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class GameRepository : IListRepository<Game>
    {
        private readonly StoreContext _context;
        public GameRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(Game entity)
        {
            await _context.Games.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
                _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Game>> GetAll()
        {
             return await _context.Games.ToListAsync();
        }

        public async Task<Game?> GetById(Guid? id)
        {
            return await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
        }

        public void Update(Game entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
