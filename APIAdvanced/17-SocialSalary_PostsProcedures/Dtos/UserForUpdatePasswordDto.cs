using System.ComponentModel.DataAnnotations;

namespace SocialSalary.Dtos
{
    public class UserForUpdatePasswordDto
    {
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? OldPassword { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? PasswordConfirm { get; set; }
    }
}