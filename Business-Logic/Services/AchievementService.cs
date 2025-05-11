using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access.Interfaces;
using Data_Access.Models;

namespace Business_Logic.Services
{
    public class AchievementService
    {
        public readonly IUnitOfWork database;
        public AchievementService(IUnitOfWork unitOfWork)
        {
            database = unitOfWork;
        }

        public async Task<IEnumerable<Achievement>> GetAll(Guid gameid)
        {
            return await database.AchievementRepository.GetAll(gameid);
        }

        public async Task AddAchievement(Achievement newAchi)
        {
            await database.AchievementRepository.Add(newAchi);
            await database.Save();
        }
        public async Task DeleteAchievement(Guid id)
        {
            await database.AchievementRepository.Delete(id);
            await database.Save();
        }
        public async Task<Achievement> GetById(Guid id)
        {
            Achievement achi = await database.AchievementRepository.GetById(id);
            return achi;
        }
    }
}
