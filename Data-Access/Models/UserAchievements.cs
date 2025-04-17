namespace Data_Access.Models

{
    public class UserAchievements
    {
        public Guid UserId { get; set; } 
        public User User { get; set; } = null!;
        public Guid AchievementId { get; set; }
        public Achievement Achievement { get; set; } = null!;
    }
}
