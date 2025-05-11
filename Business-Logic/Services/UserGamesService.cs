using AutoMapper;
using Data_Access.Interfaces;
using Data_Access.Models;
using Data_Transfer_Object.DTO.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Business_Logic.Services
{
    public class UserGamesService
    {
        private readonly IUnitOfWork Database;
        private readonly GameService _gameService;
        private readonly ILogger<UserGamesService> _logger;
        public UserGamesService(IUnitOfWork unitOfWork,GameService gameService,ILogger<UserGamesService> logger)
        {
            Database = unitOfWork;
            _gameService = gameService;
            _logger = logger;
        }
        public async Task AddUserGame(UserGame userGame)
        {
            try
            {
                await Database.UserGameRepository.Add(userGame);
            }catch(DbUpdateException)
            {
                throw;
            }
            await Database.Save();
        }

        public async Task<IEnumerable<Game>> GetAllUserGames(Guid userId)
        {
            var allGames = await Database.UserGameRepository.GetAll(userId);
            return allGames.Select(x => x.Game);
        }
    }
}
