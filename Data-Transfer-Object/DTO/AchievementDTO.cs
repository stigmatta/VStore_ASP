using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
namespace Data_Transfer_Object.DTO.Achievement
{
    public class AchievementDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
    }
}
