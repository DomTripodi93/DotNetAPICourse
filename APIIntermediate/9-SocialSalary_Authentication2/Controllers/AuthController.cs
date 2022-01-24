using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SocialSalary.Data;
using SocialSalary.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SocialSalary.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContextDapper _dapper;
        public AuthController(IConfiguration config)
        {
            _config = config;
            _dapper = new DataContextDapper(config);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
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

                string sqlAddAuth = "INSERT INTO TutorialAppSchema.Auth (Email, PasswordHash, PasswordSalt) Values ('"
                                        + userForRegisterDto.Email.ToLower() + "', @PasswordHash, @PasswordSalt)";

                List<SqlParameter> sqlParams = new List<SqlParameter>();

                SqlParameter pwParam = new SqlParameter("@PasswordHash", SqlDbType.VarBinary, 8000);
                pwParam.Value = passwordHash;

                sqlParams.Add(pwParam);

                SqlParameter saltParam = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary, 8000);
                saltParam.Value = passwordSalt;

                sqlParams.Add(saltParam);

                if (_dapper.ExecuteSQLWithParams(sqlAddAuth, sqlParams) > 0)
                {
                    string sqlCheckUserExists = "SELECT Email FROM TutorialAppSchema.Users WHERE Email = '"
                        + userForRegisterDto.Email.ToLower() + "'";

                    IEnumerable<string> userExists = _dapper.LoadData<string>(sqlCheckUserExists);

                    if (userExists.Count() == 0)
                    {
                        string sqlAddUser = "INSERT INTO TutorialAppSchema.Users (FirstName"
                                                + ",LastName"
                                                + ",Email"
                                                + ",Gender"
                                                + ",Active)"
                                                + "VALUES"
                                                + "('" + userForRegisterDto.FirstName?.Replace("'", "''")
                                                + "', '" + userForRegisterDto.LastName?.Replace("'", "''")
                                                + "', '" + userForRegisterDto.Email?.Replace("'", "''")
                                                + "', '" + userForRegisterDto.Gender
                                                + "', 1)";
                        if (_dapper.ExecuteSQL(sqlAddUser) > 0)
                        {
                            return Ok();
                        }
                        throw new Exception("User registration failed on user setup.");
                    }
                    return Ok();
                }

                throw new Exception("User registration failed on auth setup.");
            }

            throw new Exception("Passwords do not match!");
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            if (userForLoginDto.Password != null && userForLoginDto.Email != null)
            {

                string sqlGetLoginConfirm = "SELECT PasswordHash, PasswordSalt FROM TutorialAppSchema.Auth WHERE Email = '"
                    + userForLoginDto.Email.ToLower() + "'";

                string sqlGetUserId = "SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '"
                    + userForLoginDto.Email.ToLower() + "'";

                UserLoginConfirmDto loginConfirm = _dapper.LoadDataSingle<UserLoginConfirmDto>(sqlGetLoginConfirm);

                int userId = _dapper.LoadDataSingle<int>(sqlGetUserId);

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

                    return Ok(new { token = CreateToken(userId) });

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

            return Ok(CreateToken(userId.Value));
        }

        private string CreateToken(int userId)
        {
            Claim[] claims = new Claim[]
            {
                // new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                new Claim("userId", userId.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}