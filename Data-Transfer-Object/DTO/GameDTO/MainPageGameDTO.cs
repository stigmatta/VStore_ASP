namespace Data_Transfer_Object.DTO.Game
{
    public class MainPageGameDTO
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int? Discount { get; set; }
        public string LogoLink { get; set; }
        public string Developer { get; set; }
        public string? Publisher { get; set; }
        public DateOnly ReleaseDate { get; set; }

    }
}
