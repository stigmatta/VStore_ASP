namespace Data_Access.Models
{
    public class Wishlist
    {
        public Guid UserId { get; set; }
        public User User { get;set; } = null!;
        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
    }
}
