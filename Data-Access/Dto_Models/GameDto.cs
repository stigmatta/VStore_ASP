using Microsoft.AspNetCore.Http;

namespace Data_Access.Dto_Models
{
    public class GameDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public IFormFile LogoFile { get; set; }
        public string Developer { get; set; }
        public Guid RecommendedRequirementId { get; set; }
        public Guid MinimumRequirementId { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public List<IFormFile> GalleryFiles { get; set; } = new();
    }
}
