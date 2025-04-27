using System.ComponentModel.DataAnnotations;

namespace Data_Transfer_Object.DTO
{
    public class RequirementDTO
    {
        [Required]
        [MaxLength(100)]
        public string? OS { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Processor { get; set; }
        [Required]
        [Range(1, 1024)]
        public int Memory { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Graphics { get; set; }

        [Required]
        public int Storage { get; set; }

        [MaxLength(200)]
        public string? Device { get; set; }
    }

    public class RecommendedRequirementDTO : RequirementDTO { }
    public class MinimumRequirementDTO : RequirementDTO { }
}
