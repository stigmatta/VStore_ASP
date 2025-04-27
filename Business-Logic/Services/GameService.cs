using Data_Access.Interfaces;
using Data_Access.Repositories;

namespace Business_Logic.Services
{
    public class GameService
    {
        private readonly IUnitOfWork Database;
        public GameService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public async Task<IEnumerable<Game>> GetAllGames()
        {
            return await Database.GameRepository.GetAll();
        }

        public async Task AddGame(Game game)
        {
            await Database.GameRepository.Add(game);
            await Database.Save();
        }
        public async Task DeleteGame(Guid id)
        {
            await Database.GameRepository.Delete(id);
            await Database.Save();
        }
        public async Task<Game> GetById(Guid id)
        {
            Game game = await Database.GameRepository.GetById(id);
            return game;
        }
    }
}
