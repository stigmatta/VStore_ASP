namespace Data_Access.Models
{
    public abstract class Requirement
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? OS { get; set; }
        public string? Processor { get; set; }
        public int Memory { get; set; }
        public string? Graphics { get; set; }
        public int Storage { get; set; }
        public string? Device { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
    }

    public class RecommendedRequirement : Requirement { }

    public class MinimumRequirement : Requirement { }

}
