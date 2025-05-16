using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class RelationshipRepository : IRepository<Relationship>
    {
        private readonly StoreContext _context;
        public RelationshipRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(Relationship entity)
        {
            await _context.Relationships.AddAsync(entity);
        }
        public async Task Delete(Guid userId,Guid friendId)
        {
            var relationships = await _context.Relationships
                .Where(r => (r.UserId == userId && r.FriendId == friendId) ||
                           (r.UserId == friendId && r.FriendId == userId))
                .ToListAsync();

            if (relationships.Count == 0)
                return;
            _context.Relationships.RemoveRange(relationships);
        }
        public async Task<Relationship?> GetById(Guid? id)
        {
            return await _context.Relationships.FindAsync(id);
        }
        public void Update(Relationship entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public async Task<IList<Relationship>> GetAll(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Relationships)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
                return user.Relationships.ToList();
            return new List<Relationship>();
        }


        public async Task<IList<Relationship>> GetFriends(Guid userId)
        {
            var friends = await _context.Relationships.Where(x => x.UserId == userId && x.Status == "Friend").Include(x => x.FriendUser).ToListAsync();
            return friends;
        }
        public async Task<IList<Relationship>> GetBlocked(Guid userId)
        {
            var blocked = await _context.Relationships.Where(x => x.UserId == userId && x.Status == "Blocked").Include(x => x.FriendUser).ToListAsync();
            return blocked;
        }
        public async Task<IList<Relationship>> GetPending(Guid userId)
        {
            var sentRequests = await _context.Relationships.Where(x => x.UserId == userId && x.Status == "Pending").Include(x=>x.FriendUser).ToListAsync();
            return sentRequests;
        }
        public async Task<string> GetStatus(Guid userId, Guid friendId)
        {
            var relationship = await _context.Relationships
                    .Where(r => (r.UserId == userId && r.FriendId == friendId) ||
                               (r.UserId == friendId && r.FriendId == userId))
                    .Select(r => r.Status)
                    .FirstOrDefaultAsync();
            return relationship ?? "Stranger";
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
