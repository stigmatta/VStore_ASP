namespace Data_Access.Models
{
    public class Requirement
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? OS { get; set; }
        public string? Processor { get; set; }
        public int Memory { get; set; }
        public string? Graphics { get; set; }
        public int Storage { get; set; }
        public string? Device { get; set; }
    }
}
