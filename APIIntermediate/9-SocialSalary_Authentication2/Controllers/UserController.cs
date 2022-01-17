using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSalary.Data;
using SocialSalary.Models;

namespace SocialSalary.Controllers;

[Authorize]
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
            + "VALUES('" + userForInsert.FirstName?.Replace("'", "''")
            + "', '" + userForInsert.LastName?.Replace("'", "''")
            + "', '" + userForInsert.Email?.Replace("'", "''")
            + "', '" + userForInsert.Gender
            + "', '" + userForInsert.Active
            + "')";

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok(userForInsert);
        }
        throw new Exception("Adding User failed on save");
    }

    [HttpPut("Users")]
    public IActionResult PutUser(Users userForUpdate)
    {
        string sql = "UPDATE TutorialAppSchema.Users SET FirstName='"
            + userForUpdate.FirstName?.Replace("'", "''")
            + "', LastName='" + userForUpdate.LastName?.Replace("'", "''")
            + "', Email='" + userForUpdate.Email?.Replace("'", "''")
            + "', Gender='" + userForUpdate.Gender
            + "', Active='" + userForUpdate.Active
            + "' WHERE UserId=" + userForUpdate.UserId;

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok(userForUpdate);
        }
        throw new Exception("Updating User failed on save");
    }

    [HttpDelete("Users/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = "DELETE FROM TutorialAppSchema.Users  WHERE UserId=" + userId;

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok();
        }
        throw new Exception("Deleting User failed on save");
    }

    [HttpGet("UserSalary/{userId}")]
    public IEnumerable<UserSalary> GetUserSalary(int userId)
    {
        return _dapper.LoadData<UserSalary>(@"SELECT UserSalary.UserId
                    , UserSalary.Salary
            FROM  TutorialAppSchema.UserSalary
                WHERE UserId = " + userId);
    }

    [HttpPost("UserSalary")]
    public IActionResult PostUserSalary(UserSalary userSalaryForInsert)
    {
        string sql = "INSERT INTO TutorialAppSchema.UserSalary ("
            + "UserId"
            + ",Salary)"
            + "VALUES(" + userSalaryForInsert.UserId
            + ", " + userSalaryForInsert.Salary
            + ")";

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok(userSalaryForInsert);
        }
        throw new Exception("Adding UserSalary failed on save");
    }

    [HttpPut("UserSalary")]
    public IActionResult PutUserSalary(UserSalary userSalaryForUpdate)
    {
        string sql = "UPDATE TutorialAppSchema.UserSalary SET Salary=" 
            + userSalaryForUpdate.Salary
            + " WHERE UserId=" + userSalaryForUpdate.UserId;

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok(userSalaryForUpdate);
        }
        throw new Exception("Updating UserSalary failed on save");
    }

    [HttpDelete("UserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = "DELETE FROM TutorialAppSchema.UserSalary  WHERE UserId=" + userId;

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok();
        }
        throw new Exception("Deleting UserSalary failed on save");
    }

    [HttpGet("UserJobInfo/{userId}")]
    public IEnumerable<UserJobInfo> GetUserJobInfo(int userId)
    {
        return _dapper.LoadData<UserJobInfo>(@"SELECT  UserJobInfo.UserId
                    , UserJobInfo.JobTitle
                    , UserJobInfo.Department
            FROM  TutorialAppSchema.UserJobInfo
                WHERE UserId = " + userId);
    }

    [HttpPost("UserJobInfo")]
    public IActionResult PostUserJobInfo(UserJobInfo userJobInfoForInsert)
    {
        string sql = "INSERT INTO TutorialAppSchema.UserJobInfo ("
            + "UserId"
            + ",Department"
            + ",JobTitle)"
            + "VALUES(" + userJobInfoForInsert.UserId
            + ", '" + userJobInfoForInsert.Department
            + "', '" + userJobInfoForInsert.JobTitle
            + "')";

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok(userJobInfoForInsert);
        }
        throw new Exception("Adding UserJobInfo failed on save");
    }

    [HttpPut("UserJobInfo")]
    public IActionResult PutUserJobInfo(UserJobInfo userJobInfoForUpdate)
    {
        string sql = "UPDATE TutorialAppSchema.UserJobInfo SET Department='" 
            + userJobInfoForUpdate.Department
            + "', JobTitle='"
            + userJobInfoForUpdate.JobTitle
            + "' WHERE UserId=" + userJobInfoForUpdate.UserId;

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok(userJobInfoForUpdate);
        }
        throw new Exception("Updating UserJobInfo failed on save");
    }

    [HttpDelete("UserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = "DELETE FROM TutorialAppSchema.UserJobInfo  WHERE UserId=" + userId;

        if (_dapper.ExecuteSQL(sql) > 0)
        {
            return Ok();
        }
        throw new Exception("Deleting UserJobInfo failed on save");
    }
}

