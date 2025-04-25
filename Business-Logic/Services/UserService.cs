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

        public async Task<bool> VerifyUser(string username,string Password)
        {
            var users = await Database.UserRepository.GetAll();
            var userExists = users.FirstOrDefault(x => x.Username == username);
            if(userExists == null)
                return false;
            return PasswordHash.ArgonHashStringVerify(userExists.Password, Password);
        }
    }
}
