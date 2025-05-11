using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sodium;

namespace Business_Logic.Services
{
    public class UserService
    {
        IUnitOfWork Database;
        ILogger<UserService> _logger;
        public UserService(IUnitOfWork database,ILogger<UserService>logger)
        {
            Database = database;
            _logger = logger;
        }
        public async Task Create(User user)
        {
            try
            {
                user.Password = HashPassword(user.Password);
                await Database.UserRepository.Add(user);

            }catch(DbUpdateException ex)
            {
                throw;
            }
            await Database.Save();
        }
        public async Task<User?> GetById(Guid? id)
        {
            _logger.LogInformation($"Get user by id {id}");
            return await Database.UserRepository.GetById(id);
        }
        public string HashPassword(string password)
        {
            return PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Interactive);
        }

        public async Task<User?> VerifyUser(string username, string password)
        {
            var users = await Database.UserRepository.GetAll();
            var user = users.FirstOrDefault(x => x.Username == username);

            if (user == null || !PasswordHash.ArgonHashStringVerify(user.Password, password))
                return null; 

            return user; 
        }

    }
}
