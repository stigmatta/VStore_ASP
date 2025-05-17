using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sodium;

namespace Business_Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _database;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork database, ILogger<UserService> logger)
        {
            _database = database;
            _logger = logger;
        }

        public async Task Create(User user)
        {
            try
            {
                user.Password = HashPassword(user.Password);
                await _database.UserRepository.Add(user);
                await _database.Save();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creating user");
                throw;
            }
        }

        public async Task<User?> GetById(Guid? id)
        {
            return await _database.UserRepository.GetById(id);
        }

        public string HashPassword(string password)
        {
            return PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Interactive);
        }

        public async Task<User?> VerifyUser(string username, string password)
        {
            var users = await _database.UserRepository.GetAll();
            var user = users.FirstOrDefault(x => x.Username == username);

            if (user == null || !PasswordHash.ArgonHashStringVerify(user.Password, password))
                return null;

            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _database.UserRepository.GetAll();
        }

        public async Task Update(User user)
        {
            try
            {
                _database.UserRepository.Update(user);
                await _database.Save();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating user");
                throw;
            }
        }

        public async Task UpdatePassword(Guid userId, string newPassword)
        {
            try
            {
                var user = await _database.UserRepository.GetById(userId);
                if (user != null)
                {
                    user.Password = HashPassword(newPassword);
                    _database.UserRepository.Update(user);
                    await _database.Save();
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating password");
                throw;
            }
        }

        public async Task UpdateAvatar(Guid userId, string avatarUrl)
        {
            try
            {
                var user = await _database.UserRepository.GetById(userId);
                if (user != null)
                {
                    user.Photo = avatarUrl;
                    _database.UserRepository.Update(user);
                    await _database.Save();
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating avatar");
                throw;
            }
        }

        public async Task UpdateUsername(Guid userId, string newUsername)
        {
            try
            {
                var user = await _database.UserRepository.GetById(userId);
                if (user != null)
                {
                    user.Username = newUsername;
                    _database.UserRepository.Update(user);
                    await _database.Save();
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating username");
                throw;
            }
        }

        public async Task<bool> CheckUsernameExists(string username)
        {
            var users = await _database.UserRepository.GetAll();
            return users.Any(u => u.Username == username);
        }
    }
}