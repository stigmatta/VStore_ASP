using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Transfer_Object.DTO.NewsDTO
{
    public class NewsDTO
    {
        [Required]
        [MaxLength(1000)]
        public string Title { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
        [Required]
        public string Photo { get; set; }
    }
}
