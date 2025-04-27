using System.ComponentModel.DataAnnotations;

namespace Data_Transfer_Object.DTO.User
{
    public class LoginDTO
    {
        [Required]
        [Length(5,15,ErrorMessage ="Length of the username has to be between 5 and 15 symbols")]
        [RegularExpression("/^[a-zA-Z0-9_]+$/",ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        public string? Username { get; set; }
        [Required]
        [Length(8,20, ErrorMessage = "Length of the password has to be between 8 and 20 symbols")]
        [RegularExpression("/^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])\\w{8,20}$/",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string? Password { get; set; }
    }
}
