
using System.ComponentModel.DataAnnotations;

namespace SocialSalary.Dtos
{
    public partial class UserCompleteForRegisterDto
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public bool Active { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public decimal Salary { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? PasswordConfirm { get; set; }
    }
}