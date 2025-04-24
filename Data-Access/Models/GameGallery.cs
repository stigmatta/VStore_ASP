public class GameGallery
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Link { get; set; }
    public bool IsCover { get; set; }

    public string? Type { get; set; } 

    public Guid GameId { get; set; }
    public Game Game { get; set; } = null!;
}
