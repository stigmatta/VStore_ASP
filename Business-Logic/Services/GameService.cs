using Data_Access.Interfaces;

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
    }
}
