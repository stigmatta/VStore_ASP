using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class NewsRepository : IListRepository<News>
    {
        private readonly StoreContext _context;
        public NewsRepository(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(News entity)
        {
            await _context.News.AddAsync(entity);
        }

        public async Task Delete(Guid id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
                _context.News.Remove(news);
        }

        public async Task<News?> GetById(Guid id)
        {
            return await _context.News.FindAsync(id);
        }

        public void Update(News entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IEnumerable<News>>GetAll()
        {
            return await _context.News.ToListAsync();
        }
    }
}
