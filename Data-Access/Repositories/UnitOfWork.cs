using Data_Access.Context;
using Data_Access.Interfaces;

namespace Data_Access.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private UserRepository _userRepository;
        private GameRepository _gameRepository;
        private WishlistRepository _wishlistRepository;
        private GameGalleryRepository _gameGalleryRepository;
        private UserGamesRepository _userGameRepository;
        private UserAchievementRepository _userAchievementRepository;
        private AchievementRepository _achievementRepository;
        private FriendRepository _friendRepository;
        private ReviewRepository _reviewRepository;
        private BlockedUserRepository _blockedUserRepository;
        private NewsRepository _newsRepository;
        private MinimumRequirementRepository _minimumRequirementRepository;
        private RecommendedRequirementRepository _recommendedRequirementRepository;

        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }
        public UserRepository UserRepository {
            get
            {
                return _userRepository??new UserRepository(_context);
            }
        }

        public GameRepository GameRepository
        {
            get
            {
                return _gameRepository ?? new GameRepository(_context);
            }
        }
        

        public WishlistRepository WishlistRepository {
            get
            {
                return _wishlistRepository ?? new WishlistRepository(_context);
            }
        }

        public GameGalleryRepository GameGalleryRepository {
            get
            {
                return _gameGalleryRepository ?? new GameGalleryRepository(_context);
            }
        }

        public UserGamesRepository UserGameRepository {
            get
            {
                return _userGameRepository ?? new UserGamesRepository(_context);
            }
        }

        public UserAchievementRepository UserAchievementRepository
        {
            get
            {
                return _userAchievementRepository ?? new UserAchievementRepository(_context);
            }
        }
        public AchievementRepository AchievementRepository
        {
            get
            {
                return _achievementRepository ?? new AchievementRepository(_context);
            }
        }

        public FriendRepository FriendRepository {
            get
            {
                return _friendRepository ?? new FriendRepository(_context);
            }
        }

        public ReviewRepository ReviewRepository {
            get
            {
                return _reviewRepository ?? new ReviewRepository(_context);
            }
        }

        public BlockedUserRepository BlockedUserRepository {
            get
            {
                return _blockedUserRepository ?? new BlockedUserRepository(_context);
            }
        }

        public NewsRepository NewsRepository {
            get
            {
                return _newsRepository ?? new NewsRepository(_context);
            }
        }

        public MinimumRequirementRepository MinimumRequirementRepository {
            get
            {
                return _minimumRequirementRepository ?? new MinimumRequirementRepository(_context);
            }
        }

        public RecommendedRequirementRepository RecommendedRequirementRepository {
            get
            {
                return _recommendedRequirementRepository ?? new RecommendedRequirementRepository(_context);
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
