
namespace SocialSalary.Dtos
{
    public partial class PostForUpdateDto
    {
        public int PostId { get; set; }
        // public DateTime PostDate { get; set; }
        // public DateTime ChangeDate { get; set; }
        public string? PostTitle { get; set; }
        public string? PostContent { get; set; }        
    }
}