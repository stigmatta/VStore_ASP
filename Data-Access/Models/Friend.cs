namespace Data_Access.Models
{
    public class Friend
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid FriendId { get; set; }
        public User FriendUser { get; set; } = null!;
    }
}
