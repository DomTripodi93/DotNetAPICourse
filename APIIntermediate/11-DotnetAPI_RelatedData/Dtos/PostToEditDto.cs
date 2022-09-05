namespace DotnetAPI.Models
{
    public partial class PostToEditDto
    {
        public int PostId {get; set;}
        public string PostTitle {get; set;}
        public string PostContent {get; set;}

        public PostToEditDto()
        {
            if (PostTitle == null)
            {
                PostTitle = "";
            }
            if (PostContent == null)
            {
                PostContent = "";
            }
        }
    }
}