namespace Data_Transfer_Object.DTO.Game
{
    public class MainPageGameDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int? Discount { get; set; }
        public string LogoLink { get; set; }
        public DateOnly ReleaseDate { get; set; }

    }
}
