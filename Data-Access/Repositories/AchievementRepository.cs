using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class AchievementRepository:IRepository<Achievement>
    {
        private readonly StoreContext _context;
        public AchievementRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(Achievement entity)
        {
            await _context.Achievements.AddAsync(entity);
        }
        public async Task Delete(Guid id)
        {
            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement != null)
                _context.Achievements.Remove(achievement);
        }
        public async Task<Achievement?> GetById(Guid? id)
        {
            return await _context.Achievements.FindAsync(id);
        }
        public void Update(Achievement entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }


        public async Task<IList<Achievement>> GetAll(Guid gameId)
        {
            var game = await _context.Games
                .Include(g => g.Achievements)
                .FirstOrDefaultAsync(g => g.Id == gameId);
            if (game != null)
                return game.Achievements.ToList();
            return new List<Achievement>();
        }


    }
}
