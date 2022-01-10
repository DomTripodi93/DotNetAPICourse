
namespace SocialSalary.Models
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public bool Active { get; set; }
    }
}