using Microsoft.AspNetCore.Mvc;
using SocialSalary.Data;

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
}

