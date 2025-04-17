namespace Data_Access.Models
{
    public class BlockedUsers
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid BlockedUserId { get; set; }
        public User BlockedUser { get; set; } = null!;
    }
}
