namespace DotnetAPI.Dtos
{
    public partial class PostToAddDto
    {
        public string PostTitle {get; set;}
        public string PostContent {get; set;}

        public PostToAddDto()
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