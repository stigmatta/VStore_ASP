using Microsoft.AspNet.Identity;
using System;

namespace Data_Access.Models
{
    public class User : IdentityUser<Guid>, IUser<string>
    {
        public string? Photo { get; set; }
        public int Level { get; set; }
        public virtual ICollection<UserAchievement> UserAchievements { get; set; }
        public virtual ICollection<UserGame> UserGames { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<BlockedUser> BlockedUsers { get; set; }
        public virtual ICollection<Wishlist> Wishlist { get; set; }

        string IUser<string>.Id => base.Id.ToString();
    }
}
