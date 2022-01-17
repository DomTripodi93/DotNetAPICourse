using Microsoft.AspNetCore.Mvc;
using SocialSalary.Data;
using SocialSalary.Dtos;
using SocialSalary.Models;

namespace SocialSalary.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        _config = config;
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestRoute", Name = "Test")]
    public IEnumerable<string> GetTest()
    {
        return _dapper.LoadData<string>("SELECT GETDATE()");
    }

    [HttpGet("Users")]
    public IEnumerable<Users> GetUsers()
    {
        // return _dapper.LoadData<string>("SELECT * TutorialAppSchema.Users");
        return _dapper.LoadData<Users>(@"SELECT  Users.UserId
                , Users.FirstName
                , Users.LastName
                , Users.Email
                , Users.Gender
                , Users.Active
            FROM  TutorialAppSchema.Users");
    }

    [HttpGet("SingleUser/{userId}")]
    public IEnumerable<Users> GetUser(int userId)
    {
        return _dapper.LoadData<Users>(@"SELECT  Users.UserId
                    , Users.FirstName
                    , Users.LastName
                    , Users.Email
                    , Users.Gender
                    , Users.Active
            FROM  TutorialAppSchema.Users
                WHERE UserId = " + userId);
    }

    [HttpPost("Users")]
    public IActionResult PostUser(UsersDto userForInsert)
    {
        string sql = "INSERT INTO TutorialAppSchema.Users ("
            + "FirstName"
            + ",LastName"
            + ",Email"
            + ",Gender"
            + ",Active)"
            + "VALUES(" + userForInsert.FirstName?.Replace("'", "''")
            + "', '" + userForInsert.LastName?.Replace("'", "''")
            + "', '" + userForInsert.Email?.Replace("'", "''")
            + "', '" + userForInsert.Gender
            + "', '" + userForInsert.Active
            + "')";

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok(userForInsert);
        }
        throw new Exception("Inserting User failed on save");
    }

    [HttpPut("Users")]
    public IActionResult PutUser(Users userForInsert)
    {
        string sql = "UPDATE TutorialAppSchema.Users SET FirstName='"
            + userForInsert.FirstName?.Replace("'", "''")
            + "', LastName='" + userForInsert.LastName?.Replace("'", "''")
            + "', Email='" + userForInsert.Email?.Replace("'", "''")
            + "', Gender='" + userForInsert.Gender
            + "', Active='" + userForInsert.Active
            + "' WHERE UserId=" + userForInsert.UserId;

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok(userForInsert);
        }
        throw new Exception("Updating User failed on save");
    }

    [HttpDelete("Users/{userId}")]
    public IActionResult PutUser(int userId)
    {
        string sql = "DELETE FROM TutorialAppSchema.Users  WHERE UserId=" + userId;

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok();
        }
        throw new Exception("Deleting User failed on save");
    }
}

