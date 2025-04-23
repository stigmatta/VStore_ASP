namespace Data_Access.Models
{
    public class BlockedUser
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid BlockedUserId { get; set; }
        public User Blocked { get; set; } = null!;
    }
}
