using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class GameGalleryRepository:IRepository<GameGallery>
    {
        private readonly StoreContext _context;
        public GameGalleryRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(GameGallery entity)
        {
            await _context.GameGalleries.AddAsync(entity);
        }
        public async Task Delete(Guid id)
        {
            var gameGallery = await _context.GameGalleries.FindAsync(id);
            if (gameGallery != null)
                _context.GameGalleries.Remove(gameGallery);
        }
        public async Task<GameGallery?> GetById(Guid id)
        {
            return await _context.GameGalleries.FindAsync(id);
        }
        public void Update(GameGallery entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public async Task<IList<GameGallery>> GetAll(Guid gameId)
        {
            var game = await _context.Games
                .Include(g => g.GameGalleries)
                .FirstOrDefaultAsync(g => g.Id == gameId);
            if (game != null)
                return game.GameGalleries.ToList();
            return new List<GameGallery>();
        }
    }
}
