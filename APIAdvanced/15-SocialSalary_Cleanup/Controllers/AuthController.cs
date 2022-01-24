using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SocialSalary.Data;
using SocialSalary.Dtos;
using SocialSalary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using System.Data;
using SocialSalary.Helpers;
using Dapper;

namespace SocialSalary.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        private readonly TokenHelper _tokenHelper;
        private readonly ReusableSql _reusableSql;
        private readonly BaseHelper _baseHelper;
        private readonly Mapper _mapper;
        public AuthController(IConfiguration config)
        {
            _config = config;
            _dapper = new DataContextDapper(config);
            _tokenHelper = new TokenHelper(config);
            _reusableSql = new ReusableSql(config);
            _baseHelper = new BaseHelper();
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserCompleteForRegisterDto, UserComplete>();
            }));
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserCompleteForRegisterDto userForRegisterDto)
        {
            if (userForRegisterDto.Password == userForRegisterDto.PasswordConfirm && userForRegisterDto.Password != null && userForRegisterDto.Email != null)
            {
                string sqlCheckLoginExists = "SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '"
                    + userForRegisterDto.Email.ToLower() + "'";

                IEnumerable<string> loginExists = _dapper.LoadData<string>(sqlCheckLoginExists);

                if (loginExists.Count() > 0)
                {
                    return BadRequest("Email already exists");
                }

                UserComplete userForUpsert = _mapper.Map<UserComplete>(userForRegisterDto);
                userForUpsert = _reusableSql.UpsertUser(userForUpsert);

                if (userForUpsert.UserId != 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using (var rngCsp = RandomNumberGenerator.Create())
                    {
                        rngCsp.GetNonZeroBytes(passwordSalt);
                    }

                    string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordHash").Value + Convert.ToBase64String(passwordSalt);

                    byte[] passwordHash = _baseHelper.GetHash(userForRegisterDto.Password, passwordSaltPlusString);

                    string sqlAddAuth = "EXEC TutorialAppSchema.spRegistration_Upsert @Email=@EmailSupplied, @PasswordHash=@PasswordHashSupplied, @PasswordSalt=@PasswordSaltSupplied, @UserId=@UserIdSupplied";

                    List<SqlParameter> sqlParams = new List<SqlParameter>();

                    SqlParameter pwParam = new SqlParameter("@PasswordHashSupplied", SqlDbType.VarBinary, 8000);
                    pwParam.Value = passwordHash;

                    sqlParams.Add(pwParam);

                    SqlParameter saltParam = new SqlParameter("@PasswordSaltSupplied", SqlDbType.VarBinary, 8000);
                    saltParam.Value = passwordSalt;

                    sqlParams.Add(saltParam);

                    SqlParameter emailParam = new SqlParameter("@EmailSupplied", SqlDbType.NVarChar, 50);
                    emailParam.Value = userForRegisterDto.Email;

                    sqlParams.Add(emailParam);

                    SqlParameter userIdParam = new SqlParameter("@UserIdSupplied", SqlDbType.Int);
                    userIdParam.Value = userForUpsert.UserId;

                    sqlParams.Add(userIdParam);

                    if (_dapper.ExecuteSQLWithParams(sqlAddAuth, sqlParams) > 0)
                    {
                        return Ok(userForUpsert);
                    }
                    throw new Exception("Setting Authentication failed on save");
                }
                throw new Exception("Upserting User failed on save");
            }
            throw new Exception("Passwords do not match!");
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            if (userForLoginDto.Password != null && userForLoginDto.Email != null)
            {

                string sqlGetLoginConfirm = "TutorialAppSchema.spLoginConfirmation_Get @Email = '"
                    + userForLoginDto.Email.ToLower() + "'";

                UserLoginConfirmDto loginConfirm = _dapper.LoadDataSingle<UserLoginConfirmDto>(sqlGetLoginConfirm);

                if (loginConfirm.PasswordSalt != null && loginConfirm.PasswordHash != null)
                {
                    string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordHash").Value + Convert.ToBase64String(loginConfirm.PasswordSalt);

                    byte[] passwordHash = _baseHelper.GetHash(userForLoginDto.Password, passwordSaltPlusString);

                    for (int i = 0; i < passwordHash.Length; i++)
                    {
                        if (passwordHash[i] != loginConfirm.PasswordHash[i])
                        {
                            return StatusCode(401, "Authentication Failed");
                        }
                    }

                    return Ok(new { token = _tokenHelper.CreateToken(loginConfirm.UserId) });
                }

                throw new Exception("No salt for user password Login");
            }

            return StatusCode(401, "Please provide valid login data");
        }


        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string sqlGetUserId = "SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = "
                + User.FindFirst("userId")?.Value;

            int? userId = _dapper.LoadDataSingle<int>(sqlGetUserId);

            if (userId == null)
                return Unauthorized();

            return Ok(_tokenHelper.CreateToken(userId.Value));
        }
    }
}