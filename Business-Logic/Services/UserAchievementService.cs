using AutoMapper;
using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Services
{
    public class UserAchievementService
    {
        private readonly IUnitOfWork Database;
        private readonly GameService _gameService;
        private readonly AchievementService _achievementService;
        private readonly ILogger<UserAchievementService> _logger;
        private readonly IMapper _mapper;
        public UserAchievementService(IUnitOfWork unitOfWork, GameService gameService, ILogger<UserAchievementService> logger, IMapper mapper, AchievementService achievementService)
        {
            Database = unitOfWork;
            _gameService = gameService;
            _logger = logger;
            _mapper = mapper;
            _achievementService = achievementService;
        }
        public async Task AddUserAchievements(Guid userId, Guid gameId)
        {

            var game = await _gameService.GetById(gameId);
            var achievements = await Database.AchievementRepository.GetAll(gameId);

            var newAchievements = achievements
                .Select(a => new UserAchievement
                {
                    UserId = userId,
                    AchievementId = a.Id,
                })
                .ToList();

            if (newAchievements.Any())
            {
                await Database.UserAchievementRepository.AddRange(newAchievements);
                await Database.Save();
                _logger.LogInformation($"Added {newAchievements.Count} new achievements for user {userId}.");
            }
        }

        public async Task<IEnumerable<Achievement>> GetAllUserAchievements(Guid userId)
        {
            return await Database.UserAchievementRepository.GetAll(userId);
        }

    }
}
