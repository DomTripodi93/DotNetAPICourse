using SocialSalary.Data;
using SocialSalary.Models;

namespace SocialSalary.Helpers
{
    public class ReusableSql
    {
        private readonly DataContextDapper _dapper;
        public ReusableSql(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }
        public UserComplete UpsertUser(UserComplete userForUpdateDto)
        {
            if (userForUpdateDto.Email != null)
            {
                string userDataSQL = "EXEC TutorialAppSchema.spUser_Upsert @FirstName='"
                    + userForUpdateDto.FirstName?.Replace("'", "''")
                    + "', @LastName='" + userForUpdateDto.LastName?.Replace("'", "''")
                    + "', @Gender='" + userForUpdateDto.Gender
                    + "', @Active='" + userForUpdateDto.Active
                    + "', @Department='" + userForUpdateDto.Department?.Replace("'", "''")
                    + "', @JobTitle='" + userForUpdateDto.JobTitle?.Replace("'", "''")
                    + "', @Email='" + userForUpdateDto.Email.Replace("'", "''")
                    + "', @Salary=" + userForUpdateDto.Salary;

                if (userForUpdateDto.UserId != 0)
                {
                    userDataSQL += ", @UserId=" + userForUpdateDto.UserId;
                }

                if (_dapper.ExecuteSQL(userDataSQL) > 0)
                {
                    string sqlGetNewUserId = "SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '"
                        + userForUpdateDto.Email.ToLower() + "'";

                    userForUpdateDto.UserId = _dapper.LoadDataSingle<int>(sqlGetNewUserId);

                    return userForUpdateDto;
                }
                throw new Exception("Upserting User failed on save");
            }
            throw new Exception("No email provided!");
        }
    }
}