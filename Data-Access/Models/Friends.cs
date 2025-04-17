namespace Data_Access.Models
{
    public class Friends
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid FriendId { get; set; }
        public User Friend { get; set; } = null!;
    }
}
