using System.ComponentModel.DataAnnotations;

namespace TalabatAPI.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
       /* [RegularExpression(" ^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^A-Za-z\\d])[A-Za-z\\d[^A-Za-z\\d]]{8,}$ ",
            ErrorMessage ="password must contain 1 Uppercase ,1 Lowercase ,1 Digit and 1 special character")]*/
        public string Password { get; set; }
    }
}
