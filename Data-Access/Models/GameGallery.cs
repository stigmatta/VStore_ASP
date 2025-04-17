namespace Data_Access.Models
{
    public enum MediaType
    {
        Photo,
        Video
    }

    public class GameGallery
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Link { get; set; }
        public MediaType Type { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
    }
}
