using System.ComponentModel.DataAnnotations;

namespace VStore.DTO.User
{
    public class RegistrationDTO
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
            ErrorMessage = "Email is not valid")]

        public string? Email { get; set; }
        [Required]
        [Length(8, 20, ErrorMessage = "Length of the password has to be between 8 and 20 symbols")]
        [RegularExpression("/^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])\\w{8,20}$/",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string? Password { get; set; }
    }
}
