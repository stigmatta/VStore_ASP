namespace Data_Access.Models
{
    public class UserGames
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
        public int HoursPlayed { get; set; }
        public int CompletedPercent { get; set; }
        public DateTime LastPlayed { get; set; }
    }
}
