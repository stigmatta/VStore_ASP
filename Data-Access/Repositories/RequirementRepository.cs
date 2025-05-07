using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Repositories
{
    public class MinimumRequirementRepository : IListRepository<MinimumRequirement>
    {
        private readonly StoreContext _context;
        public MinimumRequirementRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task Add(MinimumRequirement entity)
        {
            await _context.MinimumRequirements.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var requirement = await _context.MinimumRequirements.FindAsync(id);
            if (requirement != null)
                _context.MinimumRequirements.Remove(requirement);
            await _context.SaveChangesAsync();
        }

        public async Task<MinimumRequirement?> GetById(Guid? id)
        {
            return await _context.MinimumRequirements.FindAsync(id);
        }

        public void Update(MinimumRequirement entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IEnumerable<MinimumRequirement>> GetAll()
        {
            return await _context.MinimumRequirements.ToListAsync();
        }
    }

    public class RecommendedRequirementRepository : IListRepository<RecommendedRequirement>
    {
        private readonly StoreContext _context;
        public RecommendedRequirementRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task Add(RecommendedRequirement entity)
        {
            await _context.RecommendedRequirements.AddAsync(entity);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(Guid id)
        {
            var requirement = await _context.RecommendedRequirements.FindAsync(id);
            if (requirement != null)
                _context.RecommendedRequirements.Remove(requirement);
        }

        public async Task<RecommendedRequirement?> GetById(Guid? id)
        {
            return await _context.RecommendedRequirements.FindAsync(id);
        }

        public void Update(RecommendedRequirement entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IEnumerable<RecommendedRequirement>> GetAll()
        {
            return await _context.RecommendedRequirements.ToListAsync();
        }
    }
}
