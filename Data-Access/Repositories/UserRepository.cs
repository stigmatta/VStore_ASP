using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class UserRepository: IListRepository<User>
    {
        private readonly StoreContext _context;
        public UserRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(User entity)
        {
            await _context.Users.AddAsync(entity);
        }
        public async Task Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
                _context.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .ToListAsync();
        }

        public async Task<User?> GetById(Guid? id)
        {
           return await _context.Users.FindAsync(id);
        }

        public void Update(User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
