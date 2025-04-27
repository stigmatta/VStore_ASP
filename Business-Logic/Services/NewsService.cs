using Data_Access.Interfaces;
using Data_Access.Models;
using Data_Access.Repositories;

namespace Business_Logic.Services
{
    public class NewsService
    {
        private readonly IUnitOfWork Database;
        public NewsService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public async Task<IEnumerable<News>> GetAll()
        {
            return await Database.NewsRepository.GetAll();
        }

        public async Task AddNews(News news)
        {
            await Database.NewsRepository.Add(news);
            await Database.Save();
        }
        public async Task DeleteNews(Guid id)
        {
            await Database.NewsRepository.Delete(id);
            await Database.Save();
        }
        public async Task<News> GetById(Guid id)
        {
            News news = await Database.NewsRepository.GetById(id);
            return news;
        }
    }
}
