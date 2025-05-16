using Data_Access.Interfaces;
using Data_Access.Models;

namespace Business_Logic.Services
{
    public class RelationshipService
    {
        private readonly IUnitOfWork Database;
        public RelationshipService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }
        public async Task<IEnumerable<Relationship>> GetFriends(Guid userId)
        {
            var friends = await Database.RelationshipRepository.GetFriends(userId);
            return friends;
        }
        public async Task<IEnumerable<Relationship>> GetBlocked(Guid userId)
        {
            var blocked = await Database.RelationshipRepository.GetBlocked(userId);
            return blocked;
        }
        public async Task<IEnumerable<Relationship>> GetPending(Guid userId)
        {
            var pending = await Database.RelationshipRepository.GetPending(userId);
            return pending;
        }

        public async Task<IEnumerable<Relationship>> GetAll(Guid userId)
        {
            var relationships = await Database.RelationshipRepository.GetAll(userId);
            return relationships;
        }
        public async Task<string> GetStatus(Guid userId, Guid friendId)
        {
            var status = await Database.RelationshipRepository.GetStatus(userId, friendId);
            return status;
        }
        public async Task DeleteFriend(Guid userId, Guid friendId)
        {
            if (userId != Guid.Empty && friendId!= Guid.Empty)
            {
                await Database.RelationshipRepository.Delete(userId,friendId);
                await Database.Save();
            }
        }
    }
}
