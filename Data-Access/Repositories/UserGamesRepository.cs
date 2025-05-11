using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class UserGamesRepository:IRepository<UserGame>
    {
        private readonly StoreContext _context;
        public UserGamesRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(UserGame entity)
        {
            await _context.UserGames.AddAsync(entity);
        }
        public async Task Delete(Guid id)
        {
            var userGame = await _context.UserGames.FindAsync(id);
            if (userGame != null)
                _context.UserGames.Remove(userGame);
        }
        public async Task<UserGame?> GetById(Guid? id)
        {
            return await _context.UserGames.FindAsync(id);
        }
        public void Update(UserGame entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public async Task<IList<UserGame>> GetAll(Guid userId)
        {
            return await _context.UserGames
                .Where(ug => ug.UserId == userId)
                .Include(ug => ug.Game) 
                .AsNoTracking()        
                .ToListAsync();
        }
    }
}
