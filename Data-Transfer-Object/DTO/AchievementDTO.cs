using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Data_Transfer_Object.DTO
{
    public class AchievementDTO
    {

        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        [Required] public IFormFile Photo { get; set; }
        [Required] public Guid GameId { get; set; }
    }
}
