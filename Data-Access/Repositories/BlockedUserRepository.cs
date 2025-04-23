using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{

    public class BlockedUserRepository:IRepository<BlockedUser>
    {
        private readonly StoreContext _context;
        public BlockedUserRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task Add(BlockedUser entity)
        {
            await _context.BlockedUsers.AddAsync(entity);
        }

        public async Task Delete(Guid id)
        {
            var blockedUser = await _context.BlockedUsers.FindAsync(id);
            if (blockedUser != null)
                _context.BlockedUsers.Remove(blockedUser);
        }

        public async Task<BlockedUser?> GetById(Guid id)
        {
            return await _context.BlockedUsers.FindAsync(id);
        }
        public async Task<IList<BlockedUser>> GetAll(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.BlockedUsers)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
                return user.BlockedUsers.ToList();
            return new List<BlockedUser>();
        }

        public void Update(BlockedUser entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

    }
}
