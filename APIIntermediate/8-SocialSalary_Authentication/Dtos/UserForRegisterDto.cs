using System.ComponentModel.DataAnnotations;

namespace SocialSalary.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? PasswordConfirm { get; set; }
    }
}