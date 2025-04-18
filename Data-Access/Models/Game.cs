using System.Data.SqlTypes;

namespace Data_Access.Models

{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public int? Discount { get; set; }
        public string? Logo { get; set; }
        public string? Developer { get; set; }
        public Guid RecommendedId { get; set; }
        public RecommendedRequirement RecommendedRequirement { get; set; } = null!;
        public Guid MinimumId { get; set; }
        public MinimumRequirement MinimumRequirement { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<Achievement> Achievements { get; set; } = null!;
        public ICollection<Review> Reviews { get; set; } = null!;
        public DateOnly ReleaseDate { get; set; }
        public virtual ICollection<GameGallery> GameGalleries { get; set; } = null!;

    }
}
