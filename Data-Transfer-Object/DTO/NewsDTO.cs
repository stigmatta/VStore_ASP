using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Data_Transfer_Object.DTO
{
    public class NewsDTO
    {
        [Required]
        [MaxLength(1000)]
        public string Title { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
        [Required]
        public IFormFile PhotoFile { get; set; }
    }
}
