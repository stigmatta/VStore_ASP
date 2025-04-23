using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class UserAchievementRepository:IRepository<UserAchievement>
    {
        private readonly StoreContext _context;
        public UserAchievementRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(UserAchievement entity)
        {
            await _context.UserAchievements.AddAsync(entity);
        }
        public async Task Delete(Guid id)
        {
            var userAchievement = await _context.UserAchievements.FindAsync(id);
            if (userAchievement != null)
                _context.UserAchievements.Remove(userAchievement);
        }
        public async Task<UserAchievement?> GetById(Guid id)
        {
            return await _context.UserAchievements.FindAsync(id);
        }
        public void Update(UserAchievement entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public async Task<IList<UserAchievement>> GetAll(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.UserAchievements)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
                return user.UserAchievements.ToList();
            return new List<UserAchievement>();
        }
    }
}
