using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialSalary.Data;
using SocialSalary.Dtos;
using SocialSalary.Models;

namespace SocialSalary.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CleanUserController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly DataContextDapper _dapper;

    public CleanUserController(IConfiguration config)
    {
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
        string sql = "EXEC TutorialAppSchema.spUser_Upsert @FirstName='" 
            + userForUpsert.FirstName?.Replace("'", "''")
            + "', @LastName='" + userForUpsert.LastName?.Replace("'", "''")
            + "', @Gender='" + userForUpsert.Gender
            + "', @Active='" + userForUpsert.Active
            + "', @Department='" + userForUpsert.Department?.Replace("'", "''")
            + "', @JobTitle='" + userForUpsert.JobTitle?.Replace("'", "''")
            + "', @Email='" + userForUpsert.Email?.Replace("'", "''")
            + "', @Salary=" + userForUpsert.Salary;

        if (userForUpsert.UserId != 0)
        {
            sql += ", @UserId=" + userForUpsert.UserId;
        }

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok(userForUpsert);
        }
        throw new Exception("Upserting User failed on save");
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

