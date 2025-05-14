namespace Data_Transfer_Object.DTO
{
    public class ReviewDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public bool IsLiked { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }

    }
}
