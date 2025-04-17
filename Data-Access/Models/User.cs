namespace Data_Access.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Photo { get; set; }
        public int Level { get; set; }
    }
}
