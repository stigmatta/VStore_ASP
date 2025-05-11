using Data_Access.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Interfaces
{
    public interface IUnitOfWork
    {
        UserRepository UserRepository { get; }
        GameRepository GameRepository { get; }
        WishlistRepository WishlistRepository { get; }
        GameGalleryRepository GameGalleryRepository { get; }
        UserGamesRepository UserGameRepository { get; }
        UserAchievementRepository UserAchievementRepository { get; }
        AchievementRepository AchievementRepository { get; }
        FriendRepository FriendRepository { get; }
        ReviewRepository ReviewRepository { get; }
        BlockedUserRepository BlockedUserRepository { get; }
        NewsRepository NewsRepository { get; }
        MinimumRequirementRepository MinimumRequirementRepository { get; }
        RecommendedRequirementRepository RecommendedRequirementRepository { get; }
        Task Save();
    }
}
