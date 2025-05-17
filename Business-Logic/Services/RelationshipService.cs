using Data_Access.Interfaces;
using Data_Access.Migrations;
using Data_Access.Models;
using Microsoft.Extensions.Logging;

namespace Business_Logic.Services
{
    public class RelationshipService
    {
        private readonly IUnitOfWork Database;
        private readonly ILogger<RelationshipService> _logger;
        public RelationshipService(IUnitOfWork unitOfWork,ILogger<RelationshipService> logger)
        {
            Database = unitOfWork;
            _logger = logger;
        }
        public async Task<Relationship> GetRelationship(Guid userId, Guid friendId)
        {
            var relationship = await Database.RelationshipRepository.GetRelation(userId, friendId);
            return relationship;
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
            if (userId == friendId)
            {
                return "Self";
            }
            var status = await Database.RelationshipRepository.GetRelation(userId, friendId);
            var inverseStatus = await Database.RelationshipRepository.GetRelation(friendId, userId);

            if (status == null && inverseStatus == null)
            {
                return "Stranger";
            }
            if (status?.Status == "Pending")
            {
                return "Pending";
            }
            if (inverseStatus?.Status == "Pending" && status?.Status == null)
            {
                return "Stranger";
            }
            return status?.Status ?? "Stranger";
        }
        public async Task<string> AddFriend(Guid userId, Guid friendId) 
        {
            if (userId != Guid.Empty && friendId != Guid.Empty)
            {
                var relationship = await Database.RelationshipRepository.GetRelation(friendId, userId);
                _logger.LogWarning($"Status: {relationship?.Status}");
                string status;
                if(relationship==null)
                    status = "Stranger";
                else
                    status = relationship.Status;

                if (status=="Stranger")
                {
                    status = "Pending";

                    await Database.RelationshipRepository.Add(new Relationship
                    {
                        UserId = userId,
                        FriendId = friendId,
                        Status = status
                    });
                }
                else if(status == "Pending")
                {
                    status = "Friend";
                    relationship.Status = status;
                    Database.RelationshipRepository.Update(relationship);

                    await Database.RelationshipRepository.Add(new Relationship
                    {
                        UserId = userId,
                        FriendId = friendId,
                        Status = status
                    });
                }
                await Database.Save();
                return status;
            }
            return "Stranger";
        }
        public async Task DeleteFriend(Guid userId, Guid friendId)
        {
            if (userId != Guid.Empty && friendId!= Guid.Empty)
            {
                await Database.RelationshipRepository.Delete(userId,friendId);
                await Database.Save();
            }
        }

        public async Task BlockUser(Guid userId, Guid friendId)
        {
            if (userId != Guid.Empty && friendId != Guid.Empty)
            {
                var relationship = await Database.RelationshipRepository.GetRelation(userId, friendId);
                var inverseRelationship = await Database.RelationshipRepository.GetRelation(friendId, userId);
                if(inverseRelationship != null)
                {
                    if(inverseRelationship.Status!= "Blocked")
                        await Database.RelationshipRepository.Delete(friendId, userId);
                }
                if (relationship == null)
                {
                    await Database.RelationshipRepository.Add(new Relationship
                    {
                        UserId = userId,
                        FriendId = friendId,
                        Status = "Blocked"
                    });
                }
                else
                {
                    relationship.Status = "Blocked";
                    Database.RelationshipRepository.Update(relationship);
                }
                await Database.Save();
            }
        }
        public async Task UnblockUser(Guid userId, Guid friendId)
        {
            if (userId != Guid.Empty && friendId != Guid.Empty)
            {
                var relationship = await Database.RelationshipRepository.GetRelation(userId, friendId);
                if (relationship != null)
                {
                    relationship.Status = "Stranger";
                    Database.RelationshipRepository.Update(relationship);
                    await Database.Save();
                }
            }
        }
    }
}
