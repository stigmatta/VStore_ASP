using System.Data.SqlTypes;

namespace Data_Access.Models

{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Title { get; set; }
        public SqlMoney SqlMoney { get; set; }
        public int? Discount { get; set; }
        public string? Logo { get; set; }
        public string? Developer { get; set; }
        public Guid RecommendedId { get; set; }
        public Requirement RecommendedRequirement { get; set; } = null!;
        public Guid MinimumId { get; set; }
        public Requirement MinimumdRequirement { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<Review> Reviews { get; set; } = null!;
        public DateOnly ReleaseDate { get; set; }
        public ICollection<Achievement> Achievements { get; set; } = null!;
        public Guid CoverId { get; set; }
        public GameGallery Cover { get; set; } = null!;

    }
}
