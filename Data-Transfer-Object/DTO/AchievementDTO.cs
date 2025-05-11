using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Data_Transfer_Object.DTO
{
    public class AchievementDTO
    {
        [Required(ErrorMessage = "Название ачивки обязательно")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ID игры обязательно")]
        public Guid GameId { get; set; }

        [Required(ErrorMessage = "Изображение обязательно")]
        public IFormFile Photo { get; set; }
    }
}