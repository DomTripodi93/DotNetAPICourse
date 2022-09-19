using System.Data;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserCompleteController : ControllerBase
{
    DataContextDapper _dapper;
    public UserCompleteController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }
    
    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers/{userId}/{isActive}")]
    public IEnumerable<UserComplete> GetUsers(int userId, bool isActive)
    {
        string sql = @"EXEC TutorialAppSchema.spUsers_Get";
        string stringParameters = "";
        DynamicParameters sqlParameters = new DynamicParameters();
        
        if (userId != 0)
        {
            stringParameters += ", @UserId=@UserIdParameter";
            sqlParameters.Add("@UserIdParameter", userId, DbType.Int32 );
        } 
        if (isActive)
        {
            stringParameters += ", @Active=@ActiveParameter";
            sqlParameters.Add("@ActiveParameter", isActive, DbType.Boolean );
        }

        if (stringParameters.Length > 0)
        {
            sql += stringParameters.Substring(1);//, parameters.Length);
        }

        IEnumerable<UserComplete> users = _dapper.LoadDataWithParameters<UserComplete>(sql, sqlParameters);
        return users;
    }
    
    [HttpPut("UpsertUser")]
    public IActionResult UpsertUser(UserComplete user)
    {
        string sql = @"EXEC TutorialAppSchema.spUser_Upsert
            @FirstName = @FirstNameParameter, 
            @LastName = @LastNameParameter, 
            @Email = @EmailParameter, 
            @Gender = @GenderParameter, 
            @Active = @ActiveParameter, 
            @JobTitle = @JobTitleParameter, 
            @Department = @DepartmentParameter, 
            @Salary = @SalaryParameter, 
            @UserId = @UserIdParameter";

        DynamicParameters sqlParameters = new DynamicParameters();

        sqlParameters.Add("@FirstNameParameter", user.FirstName, DbType.String);
        sqlParameters.Add("@LastNameParameter", user.LastName, DbType.String);
        sqlParameters.Add("@EmailParameter", user.Email, DbType.String);
        sqlParameters.Add("@GenderParameter", user.Gender, DbType.String);
        sqlParameters.Add("@ActiveParameter", user.Active, DbType.Boolean);
        sqlParameters.Add("@JobTitleParameter", user.JobTitle, DbType.String);
        sqlParameters.Add("@DepartmentParameter", user.Department, DbType.String);
        sqlParameters.Add("@SalaryParameter", user.Salary, DbType.Decimal);
        sqlParameters.Add("@UserIdParameter", user.UserId, DbType.Int32);

        if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
        {
            return Ok();
        } 

        throw new Exception("Failed to Update User");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"TutorialAppSchema.spUser_Delete
            @UserId = @UserIdParameter";

        DynamicParameters sqlParameters = new DynamicParameters();
        sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);

        if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
        {
            return Ok();
        } 

        throw new Exception("Failed to Delete User");
    }
}
