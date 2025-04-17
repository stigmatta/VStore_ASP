namespace Data_Access.Models
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
        public string? Text { get; set; }
        public int Rating { get; set; }
        public bool IsLiked { get; set; }
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    }
}
