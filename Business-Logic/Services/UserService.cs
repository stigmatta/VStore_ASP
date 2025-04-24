using Data_Access.Interfaces;
using Data_Access.Models;
using Microsoft.EntityFrameworkCore;
using Sodium;

namespace Business_Logic.Services
{
    public class UserService
    {
        IUnitOfWork Database;
        public UserService(IUnitOfWork database)
        {
            Database = database;
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
        public string HashPassword(string password)
        {
            return PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Interactive);
        }
        
    }
}
