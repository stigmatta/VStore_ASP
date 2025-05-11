using Data_Transfer_Object.DTO.Game;

namespace Data_Transfer_Object.DTO.GameDTO
{
    public class ProfileGameDTO:MainPageGameDTO
    {
        public virtual ICollection<AchievementDTO> Achievements { get; set; }
        public int HoursPlayed { get; set; }
        public int CompletedPercent { get; set; }
        public DateTime LastPlayed { get; set; }
    }
}
