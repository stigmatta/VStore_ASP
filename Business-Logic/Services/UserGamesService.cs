using AutoMapper;
using Data_Access.Interfaces;
using Data_Access.Models;
using Data_Transfer_Object.DTO.Achievement;
using Data_Transfer_Object.DTO.GameDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Business_Logic.Services
{
    public class UserGamesService
    {
        private readonly IUnitOfWork Database;
        private readonly GameService _gameService;
        private readonly AchievementService _achievementService;
        private readonly ILogger<UserGamesService> _logger;
        private readonly IMapper _mapper;
        private readonly UserAchievementService _userAchievementService;
        public UserGamesService(IUnitOfWork unitOfWork,GameService gameService,ILogger<UserGamesService> logger,
            IMapper mapper,AchievementService achievementService,UserAchievementService userAchievementService)
        {
            Database = unitOfWork;
            _gameService = gameService;
            _logger = logger;
            _mapper = mapper;
            _achievementService = achievementService;
            _userAchievementService = userAchievementService;
        }
        public async Task AddUserGame(UserGame userGame)
        {
            try
            {
                await Database.UserGameRepository.Add(userGame);
                await _userAchievementService.AddUserAchievements(userGame.UserId, userGame.GameId);

            }
            catch (DbUpdateException)
            {
                throw;
            }
            await Database.Save();
        }

        public async Task<IEnumerable<UserGame>> GetAllUserGames(Guid userId)
        {
            var allGames = await Database.UserGameRepository.GetAll(userId);
            return allGames;
        }
        public async Task<IEnumerable<ProfileGameDTO>> MapUserGames(IEnumerable<UserGame> games)
        {
            var profileGameDTOs = new List<ProfileGameDTO>();
            foreach (var game in games)
            {
                var gameDTO = new ProfileGameDTO
                {
                    Id = game.GameId,
                    Title = game.Game.Title,
                    LogoLink = game.Game.Logo,
                    Price = game.Game.Price,
                    Discount = game.Game.Discount,
                    ReleaseDate = game.Game.ReleaseDate,
                    Achievements = _mapper.Map<IList<AchievementDTO>>(await _achievementService.GetAll(game.GameId)),
                    HoursPlayed = game.HoursPlayed,
                    CompletedPercent = game.CompletedPercent,
                    LastPlayed = game.LastPlayed
                };
                profileGameDTOs.Add(gameDTO);
            }
            return profileGameDTOs;
        }
    }
}
