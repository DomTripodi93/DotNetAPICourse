using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly DataContextEF _entityFramework;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;

    public UserEFController(IConfiguration config, DataContextEF entityFramework, IUserRepository userRepo)
    {
        _config = config;
        _entityFramework = entityFramework;
        _userRepo = userRepo;
        _mapper = new Mapper(new MapperConfiguration(cfg => { 
            cfg.CreateMap<UsersForCreateDto, Users>().ReverseMap();
            cfg.CreateMap<Users, Users>();
            cfg.CreateMap<UserSalary, UserSalary>();
            cfg.CreateMap<UserJobInfo, UserJobInfo>();
        }));
    }

    [HttpGet("UsersEF")]
    public async Task<IEnumerable<Users>> GetUsersEf()
    {
        return await _userRepo.GetUsers();
        // return await _entityFramework.Users.ToListAsync();
    }
    
    [HttpGet("SingleUserEF/{userId}")]
    public async Task<Users> GetUsersEF(int userId)
    {
        return await _userRepo.GetUser(userId);
        //  _entityFramework.Users
        //     .Where(u => u.UserId == userId)
        //     .ToListAsync();
    }

    [HttpPost("UsersEF")]
    public async Task<IActionResult> PostUsersEf(UsersForCreateDto userForInsert)
    {
        Users mappedUserForInsert = _mapper.Map<Users>(userForInsert);
        // _entityFramework.Users.Add(mappedUserForInsert);
        _userRepo.Add(mappedUserForInsert);
        if (await _userRepo.SaveAll())
        // if (await _entityFramework.SaveChangesAsync() > 0)
        {
            return Ok();
        }
        throw new Exception("Adding User failed on save");
    }


    [HttpPut("UsersEF")]
    public async Task<IActionResult> PutUsersEf(Users userForUpdate)
    {
        Users userToUpdate = await _userRepo.GetUser(userForUpdate.UserId);
        // Users userToUpdate = await _entityFramework.Users.Where(u => u.UserId == userForUpdate.UserId).FirstOrDefaultAsync();

        if (userToUpdate != null)
        {
            _mapper.Map(userToUpdate, userForUpdate);
            if (await _userRepo.SaveAll())
            // if (await _entityFramework.SaveChangesAsync() > 0)
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
            _userRepo.Delete(userToDelete);
            // _entityFramework.Users.Remove(userToDelete);
            if (await _userRepo.SaveAll())
            // if (await _entityFramework.SaveChangesAsync() > 0)
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

