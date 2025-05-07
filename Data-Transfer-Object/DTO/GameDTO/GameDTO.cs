
namespace Data_Transfer_Object.DTO.Game
{
    public class GameDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? Discount { get; set; }
        public string LogoLink { get; set; }
        public string Developer { get; set; }
        public string? Publisher { get; set; }
        public string? PEGI { get; set; }
        public string? TrailerLink { get; set; }
        public Guid? RecommendedRequirementId { get; set; }
        public Guid? MinimumRequirementId { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public List<string> Gallery { get; set; } = new();
    }
}
