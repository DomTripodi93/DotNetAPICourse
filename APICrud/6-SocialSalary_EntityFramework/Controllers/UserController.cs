using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    private readonly DataContextEF _entityFramework;
    private readonly IMapper _mapper;

    public UserController(IConfiguration config, DataContextEF entityFramework, IMapper mapper)
    {
        _config = config;
        _dapper = new DataContextDapper(config);
        _entityFramework = entityFramework;
        _mapper = new Mapper(new MapperConfiguration(cfg => { 
            cfg.CreateMap<UsersDto, Users>().ReverseMap();
            cfg.CreateMap<Users, Users>();
            cfg.CreateMap<UserSalary, UserSalary>();
            cfg.CreateMap<UserJobInfo, UserJobInfo>();
        }));
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
    public IEnumerable<Users> GetUsers(int userId)
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
    public IActionResult PostUser(Users userForInsert)
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
            + userJobInfoForUpdate.Department
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

    //Converted to Entity Framework

    [HttpGet("UsersEF")]
    public async Task<IEnumerable<Users>> GetUsersEf()
    {
        return await _entityFramework.Users.ToListAsync();
    }
    
    [HttpGet("SingleUserEF/{userId}")]
    public async Task<IEnumerable<Users>> GetUsersEF(int userId)
    {
        return await _entityFramework.Users
            .Where(u => u.UserId == userId)
            .ToListAsync();
    }

    [HttpPost("UsersEF")]
    public async Task<IActionResult> PostUsersEf(UsersDto userForInsert)
    {
        Users mappedUserForInsert = _mapper.Map<Users>(userForInsert);
        _entityFramework.Users.Add(mappedUserForInsert);
        if (await _entityFramework.SaveChangesAsync() > 0)
        {
            return Ok();
        }
        throw new Exception("Adding User failed on save");
    }


    [HttpPut("UsersEF")]
    public async Task<IActionResult> PutUsersEf(Users userForUpdate)
    {
        Users userToUpdate = await _entityFramework.Users.Where(u => u.UserId == userForUpdate.UserId).FirstOrDefaultAsync();

        if (userToUpdate != null)
        {
            _mapper.Map(userToUpdate, userForUpdate);
            if (await _entityFramework.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            throw new Exception("Updating User failed on save");
        }
        throw new Exception("Failed to find User to Update");
    }


    [HttpDelete("UsersEF/{userId}")]
    public async Task<IActionResult> DeleteUsersEf(int userId)
    {
        Users userToDelete = await _entityFramework.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();

        if (userToDelete != null)
        {
            _entityFramework.Users.Remove(userToDelete);
            if (await _entityFramework.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            throw new Exception("Deleting User failed on save");
        }
        throw new Exception("Failed to find User to delete");
    }

    [HttpGet("UserSalaryEF/{userId}")]
    public async Task<IEnumerable<UserSalary>> GetUserSalaryEF(int userId)
    {
        return await _entityFramework.UserSalary
            .Where(u => u.UserId == userId)
            .ToListAsync();
    }

    [HttpPost("UserSalaryEF")]
    public async Task<IActionResult> PostUserSalaryEf(UserSalary userForInsert)
    {
        _entityFramework.UserSalary.Add(userForInsert);
        if (await _entityFramework.SaveChangesAsync() > 0)
        {
            return Ok();
        }
        throw new Exception("Adding UserSalary failed on save");
    }


    [HttpPut("UserSalaryEF")]
    public async Task<IActionResult> PutUserSalaryEf(UserSalary userForUpdate)
    {
        UserSalary userToUpdate = await _entityFramework.UserSalary.Where(u => u.UserId == userForUpdate.UserId).FirstOrDefaultAsync();

        if (userToUpdate != null)
        {
            _mapper.Map(userToUpdate, userForUpdate);
            if (await _entityFramework.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            throw new Exception("Updating UserSalary failed on save");
        }
        throw new Exception("Failed to find UserSalary to Update");
    }


    [HttpDelete("UserSalaryEF/{userId}")]
    public async Task<IActionResult> DeleteUserSalaryEf(int userId)
    {
        UserSalary userToDelete = await _entityFramework.UserSalary.Where(u => u.UserId == userId).FirstOrDefaultAsync();

        if (userToDelete != null)
        {
            _entityFramework.UserSalary.Remove(userToDelete);
            if (await _entityFramework.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            throw new Exception("Deleting UserSalary failed on save");
        }
        throw new Exception("Failed to find UserSalary to delete");
    }


    [HttpGet("UserJobInfoEF/{userId}")]
    public async Task<IEnumerable<UserJobInfo>> GetUserJobInfoEF(int userId)
    {
        return await _entityFramework.UserJobInfo
            .Where(u => u.UserId == userId)
            .ToListAsync();
    }

    [HttpPost("UserJobInfoEf")]
    public async Task<IActionResult> PostUserJobInfoEf(UserJobInfo userForInsert)
    {
        _entityFramework.UserJobInfo.Add(userForInsert);
        if (await _entityFramework.SaveChangesAsync() > 0)
        {
            return Ok();
        }
        throw new Exception("Adding UserJobInfo failed on save");
    }


    [HttpPut("UserJobInfoEF")]
    public async Task<IActionResult> PutUserJobInfoEf(UserJobInfo userForUpdate)
    {
        UserJobInfo userToUpdate = await _entityFramework.UserJobInfo.Where(u => u.UserId == userForUpdate.UserId).FirstOrDefaultAsync();

        if (userToUpdate != null)
        {
            _mapper.Map(userToUpdate, userForUpdate);
            if (await _entityFramework.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            throw new Exception("Updating UserJobInfo failed on save");
        }
        throw new Exception("Failed to find UserJobInfo to Update");
    }


    [HttpDelete("UserJobInfoEF/{userId}")]
    public async Task<IActionResult> DeleteUserJobInfoEf(int userId)
    {
        UserJobInfo userToDelete = await _entityFramework.UserJobInfo.Where(u => u.UserId == userId).FirstOrDefaultAsync();

        if (userToDelete != null)
        {
            _entityFramework.UserJobInfo.Remove(userToDelete);
            if (await _entityFramework.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            throw new Exception("Deleting UserJobInfo failed on save");
        }
        throw new Exception("Failed to find UserJobInfo to delete");
    }
}

