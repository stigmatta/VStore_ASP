namespace Data_Access.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }
        public string? Photo { get; set; }
        public int Level { get; set; } = 1;
        public virtual ICollection<UserAchievement> UserAchievements { get; set; } = null!;
        public virtual ICollection<UserGame> UserGames { get; set; } = null!;
        public virtual ICollection<Relationship>Relationships { get; set; } = null!;
        public virtual ICollection<Wishlist> Wishlist { get; set; } = null!;
    }
}
