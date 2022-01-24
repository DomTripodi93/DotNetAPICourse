using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialSalary.Data;
using SocialSalary.Dtos;
using SocialSalary.Models;
using SocialSalary.Helpers;

namespace SocialSalary.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly DataContextDapper _dapper;
    private readonly ReusableSql _reusableSql;
    public UserController(IConfiguration config)
    {
        _reusableSql = new ReusableSql(config);
        _config = config;
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("{department}/{jobTitle}/{userId}")]
    public IEnumerable<UserComplete> GetUsers(string department = "None", string jobTitle = "None", int userId = 0)
    {
        string sql = "EXEC TutorialAppSchema.spUsers_Get";

        // string parameters = "";
        string parameters = String.Empty;

        if (department != "None")
        {
            parameters += ", @Department='" + department + "'";
        }

        if (jobTitle != "None")
        {
            parameters += ", @JobTitle='" + jobTitle + "'";
        }

        if (userId != 0)
        {
            parameters += ", @userId=" + userId.ToString();
        }

        if (parameters != "")
        {
            sql += parameters.Substring(1);
        }

        return _dapper.LoadData<UserComplete>(sql);
    }

    [HttpGet("Self")]
    public IEnumerable<UserComplete> GetUser()
    {
        string sql = "EXEC TutorialAppSchema.spUsers_Get @UserId=" + User.FindFirst("userId")?.Value;

        return _dapper.LoadData<UserComplete>(sql);
    }

    [HttpPut(Name = "UpsertUser")]
    public IActionResult PutUser(UserComplete userForUpsert)
    {
        return Ok(_reusableSql.UpsertUser(userForUpsert));
    }

    [HttpDelete("Users/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = "EXEC TutorialAppSchema.spUser_Delete @UserId=" + userId;

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok();
        }
        throw new Exception("Deleting User failed on save");
    }
}

