namespace Data_Access.Models
{
    public class News
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set; }
        public string? Photo { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
