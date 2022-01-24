
namespace SocialSalary.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime ChangeDate { get; set; }
        public string? PostTitle { get; set; }
        public string? PostContent { get; set; }        
    }
}