using Data_Access.Interfaces;
using Data_Access.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business_Logic.Services
{
    public interface IRequirementsService
    {
        Task AddMinimumRequirement(MinimumRequirement requirement);
        Task DeleteMinimumRequirement(Guid id);
        Task AddRecommendedRequirement(RecommendedRequirement requirement);
        Task DeleteRecommendedRequirement(Guid id);
        Task<IEnumerable<MinimumRequirement>> GetAllMinimumRequirements();
        Task<IEnumerable<RecommendedRequirement>> GetAllRecommendedRequirements();
    }

    public class RequirementsService : IRequirementsService
    {
        private readonly IListRepository<MinimumRequirement> _minReqRepository;
        private readonly IListRepository<RecommendedRequirement> _recReqRepository;

        public RequirementsService(
            IListRepository<MinimumRequirement> minReqRepository,
            IListRepository<RecommendedRequirement> recReqRepository)
        {
            _minReqRepository = minReqRepository;
            _recReqRepository = recReqRepository;
        }

        public async Task AddMinimumRequirement(MinimumRequirement requirement)
        {
            await _minReqRepository.Add(requirement);
        }
        public async Task DeleteMinimumRequirement(Guid id)
        {
            await _minReqRepository.Delete(id);
        }
        public async Task DeleteRecommendedRequirement(Guid id)
        {
            await _recReqRepository.Delete(id);
        }

        public async Task AddRecommendedRequirement(RecommendedRequirement requirement)
        {
            await _recReqRepository.Add(requirement);
        }

        public async Task<IEnumerable<MinimumRequirement>> GetAllMinimumRequirements()
        {
            return await _minReqRepository.GetAll();
        }

        public async Task<IEnumerable<RecommendedRequirement>> GetAllRecommendedRequirements()
        {
            return await _recReqRepository.GetAll();
        }
    }
}