using Data_Access.Models;
using Microsoft.AspNetCore.Http;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public int? Discount { get; set; }
    public string? Logo { get; set; }
    public string? Developer { get; set; }
    public string? Publisher { get; set; }
    public string? PEGI { get; set; }
    public string? TrailerLink { get; set; }
    public string? Description { get; set; }
    public DateOnly ReleaseDate { get; set; }

    public Guid? RecommendedRequirementId { get; set; }
    public RecommendedRequirement? RecommendedRequirement { get; set; }

    public Guid? MinimumRequirementId { get; set; }
    public MinimumRequirement? MinimumRequirement { get; set; }

    public ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<GameGallery> GameGalleries { get; set; } = new List<GameGallery>();
}
