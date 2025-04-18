namespace Data_Access.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Photo { get; set; }
        public int Level { get; set; }
        public virtual ICollection<UserAchievements> UserAchievements { get; set; } = null!;
        public virtual ICollection<UserGames> UserGames { get; set; } = null!;
        public virtual ICollection<Friends> Friends { get; set; } = null!;
        public virtual ICollection<BlockedUsers> BlockedUsers { get; set; } = null!;
        public virtual ICollection<Wishlist> Wishlist { get; set; } = null!;
    }
}
