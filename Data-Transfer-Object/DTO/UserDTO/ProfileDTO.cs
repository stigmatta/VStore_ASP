using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Transfer_Object.DTO.UserDTO
{
    public class ProfileDTO
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Photo { get; set; }
        public int Level { get; set; } = 1;
    }
}
