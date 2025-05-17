using Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business_Logic.Services
{
    public interface IUserService
    {
        Task Create(User user);
        Task<User?> GetById(Guid? id);
        Task<User?> VerifyUser(string username, string password);
        Task<IEnumerable<User>> GetAll();
        Task Update(User user);
        Task UpdatePassword(Guid userId, string newPassword);
        Task UpdateAvatar(Guid userId, string avatarUrl);
        Task UpdateUsername(Guid userId, string newUsername);
        Task<bool> CheckUsernameExists(string username);
    }
}