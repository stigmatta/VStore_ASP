using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Transfer_Object.DTO
{
    public class NewsDTO
    {
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
        public IFormFile PhotoFile { get; set; }
    }
}
