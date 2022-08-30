using System.Text;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using DotnetAPI.Helpers;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        private readonly TokenHelper _tokenHelper;
        public AuthController(IConfiguration config)
        {
            _config = config;
            _dapper = new DataContextDapper(config);
            _tokenHelper = new TokenHelper(config);
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

                string userDataSQL = "EXEC TutorialAppSchema.spUser_Upsert @FirstName='"
                    + userForRegisterDto.FirstName?.Replace("'", "''")
                    + "', @LastName='" + userForRegisterDto.LastName?.Replace("'", "''")
                    + "', @Gender='" + userForRegisterDto.Gender
                    + "', @Active='" + userForRegisterDto.Active
                    + "', @Department='" + userForRegisterDto.Department?.Replace("'", "''")
                    + "', @JobTitle='" + userForRegisterDto.JobTitle?.Replace("'", "''")
                    + "', @Email='" + userForRegisterDto.Email.Replace("'", "''")
                    + "', @Salary=" + userForRegisterDto.Salary;

                if (_dapper.ExecuteSQL(userDataSQL) > 0)
                {
                    string sqlGetNewUserId = "SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" 
                        + userForRegisterDto.Email.ToLower() + "'";
                    userForRegisterDto.UserId = _dapper.LoadDataSingle<int>(sqlGetNewUserId);

                    byte[] passwordSalt = new byte[128 / 8];
                    using (var rngCsp = RandomNumberGenerator.Create())
                    {
                        rngCsp.GetNonZeroBytes(passwordSalt);
                    }

                    string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordHash").Value + Convert.ToBase64String(passwordSalt);

                    byte[] passwordHash = KeyDerivation.Pbkdf2(
                        password: userForRegisterDto.Password,
                        salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256 / 8);

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
                    userIdParam.Value = userForRegisterDto.UserId;

                    sqlParams.Add(userIdParam);

                    if (_dapper.ExecuteSQLWithParams(sqlAddAuth, sqlParams) > 0)
                    {
                        return Ok(userForRegisterDto);
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

                    byte[] passwordHash = KeyDerivation.Pbkdf2(
                        password: userForLoginDto.Password,
                        salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256 / 8);

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


        // [AllowAnonymous]
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