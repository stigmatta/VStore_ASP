namespace Data_Access.Models
{
    public class Achievement
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set;}
        public string? Photo { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
        public virtual ICollection<UserAchievements> UserAchievements { get; set; } = null!;
    }
}
