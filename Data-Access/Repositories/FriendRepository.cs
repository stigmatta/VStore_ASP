using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class FriendRepository:IRepository<Friend>
    {
        private readonly StoreContext _context;
        public FriendRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(Friend entity)
        {
            await _context.Friends.AddAsync(entity);
        }
        public async Task Delete(Guid id)
        {
            var friend = await _context.Friends.FindAsync(id);
            if (friend != null)
                _context.Friends.Remove(friend);
        }
        public async Task<Friend?> GetById(Guid? id)
        {
            return await _context.Friends.FindAsync(id);
        }
        public void Update(Friend entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public async Task<IList<Friend>> GetAll(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Friends)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
                return user.Friends.ToList();
            return new List<Friend>();
        }
    }
}
