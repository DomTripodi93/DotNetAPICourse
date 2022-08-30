using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly DataContextDapper _dapper;

    public PostController(IConfiguration config)
    {
        _config = config;
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("Post")]
    public IEnumerable<Post> GetPost()
    {
        // return _dapper.LoadData<string>("SELECT * TutorialAppSchema.Post");
        return _dapper.LoadData<Post>(@"SELECT  Post.PostId
                , Post.UserId
                , Post.PostDate
                , Post.ChangeDate
                , Post.PostTitle
                , Post.PostContent
            FROM  TutorialAppSchema.Post");
    }


    [HttpGet("SinglePost/{postId}")]
    public IEnumerable<Post> GetSinglePost(int postId)
    {
        return _dapper.LoadData<Post>(@"SELECT  Post.PostId
                , Post.UserId
                , Post.PostDate
                , Post.ChangeDate
                , Post.PostTitle
                , Post.PostContent
            FROM  TutorialAppSchema.Post
                WHERE Post.PostId = " + postId);
    }


    [HttpGet("SingleUser/{userId}")]
    public IEnumerable<Post> GetPostsForUser(int userId)
    {
        return _dapper.LoadData<Post>(@"SELECT  Post.PostId
                , Post.UserId
                , Post.PostDate
                , Post.ChangeDate
                , Post.PostTitle
                , Post.PostContent
            FROM  TutorialAppSchema.Post
                WHERE Post.UserId = " + userId);
    }

    [HttpGet("MyPosts")]
    public IEnumerable<Post> GetMyPosts(int userId)
    {
        return _dapper.LoadData<Post>(@"SELECT  Post.PostId
                , Post.UserId
                , Post.PostDate
                , Post.ChangeDate
                , Post.PostTitle
                , Post.PostContent
            FROM  TutorialAppSchema.Post
                WHERE Post.UserId = " + User.FindFirst("userId")?.Value);
    }

    [HttpPost("Post")]
    public IActionResult PostPost(PostForInsertDto postForInsert)
    {
        string sql = "INSERT INTO TutorialAppSchema.Post ("
            + "UserId"
            + ",PostDate"
            + ",ChangeDate"
            + ",PostTitle"
            + ",PostContent)"
            + "VALUES('" + User.FindFirst("userId")?.Value
            + "', GETDATE(), GETDATE(), '"
            + postForInsert.PostTitle?.Replace("'", "''")
            // + "', '" + postForInsert.PostDate.ToString("O")
            // + "', '" + postForInsert.ChangeDate.ToString("O")
            // + "', '" + postForInsert.PostTitle?.Replace("'", "''")
            + "', '" + postForInsert.PostContent?.Replace("'", "''")
            + "')";

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok(postForInsert);
        }
        throw new Exception("Adding Post failed on save");
    }

    [HttpPut("Post")]
    public IActionResult PutPost(PostForUpdateDto postForUpdate)
    {
        string sqlOwnershipCheck = @"SELECT  Post.PostId
                , Post.UserId
                , Post.PostDate
                , Post.ChangeDate
                , Post.PostTitle
                , Post.PostContent
            FROM  TutorialAppSchema.Post
                WHERE Post.UserId = " + User.FindFirst("userId")?.Value
                + "AND Post.PostId=" + postForUpdate.PostId;

        if (_dapper.LoadData<Post>(sqlOwnershipCheck).Count() > 0 )
        {
            string sql = "UPDATE TutorialAppSchema.Post SET ChangeDate=GETDATE()"
                + ", PostTitle='" + postForUpdate.PostTitle?.Replace("'", "''")
                // + postForUpdate.ChangeDate.ToString("O")
                // + "', PostTitle='" + postForUpdate.PostTitle?.Replace("'", "''")
                + "', PostContent='" + postForUpdate.PostContent?.Replace("'", "''")
                + "' WHERE Post.PostId=" + postForUpdate.PostId
                + " AND UserId=" + User.FindFirst("userId")?.Value;

            if (_dapper.ExecuteSQL(sql) > 0)
            {
                return Ok(postForUpdate);
            }
            throw new Exception("Updating Post failed on save");
        }        
        throw new Exception("You can only update your own posts");
    }

    [HttpDelete("Post/{postId}")]
    public IActionResult DeletePost(int postId)
    {
        string sqlOwnershipCheck = @"SELECT  Post.PostId
                , Post.UserId
                , Post.PostDate
                , Post.ChangeDate
                , Post.PostTitle
                , Post.PostContent
            FROM  TutorialAppSchema.Post
                WHERE Post.UserId = " + User.FindFirst("userId")?.Value
                + "AND Post.PostId=" + postId;

        if (_dapper.LoadData<Post>(sqlOwnershipCheck).Count() > 0 )
        {
            string sql = "DELETE FROM TutorialAppSchema.Post  WHERE PostId=" + postId;

            if (_dapper.ExecuteSQL(sql) > 0)
            {
                return Ok();
            }
            throw new Exception("Deleting Post failed on save");
        }        
        throw new Exception("You can only delete your own posts");
    }
}

