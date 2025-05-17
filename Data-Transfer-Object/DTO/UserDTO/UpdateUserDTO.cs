
namespace Data_Transfer_Object.DTO.User
{
    public class UpdateUsernameDTO
    {
        public string NewUsername { get; set; }
    }
    public class UpdatePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
