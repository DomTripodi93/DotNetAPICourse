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

    [HttpGet("Post/{userId}/{searchFor}")]
    public IEnumerable<Post> GetPost(int userId = 0, string searchFor = "None")
    {
        string sql = "EXEC TutorialAppSchema.spPosts_Get";
        string parameters = "";

        if (userId != 0)
        {
            parameters += ", @UserId=" + userId;
        }
        if (searchFor != "None")
        {
            parameters += ", @SearchFor='" + searchFor + "'";
        }

        if (parameters != "")
        {
            sql += parameters.Substring(1);
        }

        return _dapper.LoadData<Post>(sql);
    }

    [HttpGet("MyPosts")]
    public IEnumerable<Post> GetMyPosts(int userId)
    {
        string sql = "EXEC TutorialAppSchema.spPosts_Get @UserId="
            + User.FindFirst("userId")?.Value;
        return _dapper.LoadData<Post>(sql);
    }

    [HttpPut("Post")]
    public IActionResult PutPost(PostForUpdateDto postForUpdate)
    {
        string sql = "EXEC TutorialAppSchema.spPost_Upsert @UserId="
            + User.FindFirst("userId")?.Value
            + ", @PostTitle='" + postForUpdate.PostTitle?.Replace("'", "''")
            + "', @PostContent='" + postForUpdate.PostContent?.Replace("'", "''")
            + "'";
        
        if (postForUpdate.PostId != 0)
        {
            sql += ", @PostId=" + postForUpdate.PostId;
        }

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok(postForUpdate);
        }
        throw new Exception("Post does not exist, or you do not have permission to change it");
    }

    [HttpDelete("Post/{postId}")]
    public IActionResult DeletePost(int postId)
    {
        string sql = "EXEC TutorialAppSchema.spPost_Delete @UserId = " 
            + User.FindFirst("userId")?.Value
            + ", @PostId=" + postId;

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok();
        }
        throw new Exception("Post does not exist, or you do not have permission to delete it");
    }
}

