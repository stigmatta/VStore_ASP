using Data_Access.Interfaces;

namespace Business_Logic.Services
{
    public class GameGalleryService
    {
        private readonly IUnitOfWork Database;
        public GameGalleryService(IUnitOfWork _uow) {
            Database = _uow;
        }

        public async Task AddGameGallery(GameGallery gameGallery)
        {
            await Database.GameGalleryRepository.Add(gameGallery);
            await Database.Save();
        }
        public async Task<IList<GameGallery>> GetByGameId(Guid gameId)
        {
            try
            {
                var gallery = await Database.GameGalleryRepository.GetAll(gameId);

                if (gallery == null)
                    return new List<GameGallery>();

                return gallery;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
