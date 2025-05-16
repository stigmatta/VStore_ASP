namespace Data_Access.Models
{
    public class Relationship
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid FriendId { get; set; }
        public User FriendUser { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
