using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SQLSeed.Data;
using SQLSeed.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace SQLSeed
{
    public class Program
    {
        public static void Main(string[] args)
        {

            IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            DataContextDapper dataContextDapper = new DataContextDapper(config);

            string tableCreateSql = System.IO.File.ReadAllText("Users.sql");
            dataContextDapper.ExecuteSQL(tableCreateSql);

            string usersJson = System.IO.File.ReadAllText("Users.json");

            IEnumerable<Users>? users = JsonConvert.DeserializeObject<IEnumerable<Users>>(usersJson);

            if (users != null)
            {
                using (IDbConnection dbConnection = new SqlConnection(config.GetConnectionString("DefaultConnection")))
                {
                    string sql = "SET IDENTITY_INSERT TutorialAppSchema.Users ON;"
                                    + "INSERT INTO TutorialAppSchema.Users (UserId"
                                    + ",FirstName"
                                    + ",LastName"
                                    + ",Email"
                                    + ",Gender"
                                    + ",Active)"
                                    + "VALUES";
                    foreach (Users singleUser in users)
                    {
                        string sqlToAdd = "(" + singleUser.UserId
                                    + ", '" + singleUser.FirstName?.Replace("'", "''")
                                    + "', '" + singleUser.LastName?.Replace("'", "''")
                                    + "', '" + singleUser.Email?.Replace("'", "''")
                                    + "', '" + singleUser.Gender
                                    + "', '" + singleUser.Active
                                    + "'),";

                        if ((sql + sqlToAdd).Length > 4000)
                        {
                            dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                            sql = "SET IDENTITY_INSERT TutorialAppSchema.Users ON;"
                                    + "INSERT INTO TutorialAppSchema.Users (UserId"
                                    + ",FirstName "
                                    + ",LastName"
                                    + ",Email"
                                    + ",Gender"
                                    + ",Active)"
                                    + "VALUES";
                        }
                        sql += sqlToAdd;
                    }
                    dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                }
            }
            dataContextDapper.ExecuteSQL("SET IDENTITY_INSERT TutorialAppSchema.Users OFF");

            string userSalaryJson = System.IO.File.ReadAllText("UserSalary.json");

            IEnumerable<UserSalary>? userSalary = JsonConvert.DeserializeObject<IEnumerable<UserSalary>>(userSalaryJson);

            dataContextDapper.ExecuteSQL("TRUNCATE TABLE TutorialAppSchema.UserSalary");

            if (userSalary != null)
            {
                using (IDbConnection dbConnection = new SqlConnection(config.GetConnectionString("DefaultConnection")))
                {
                    string sql = "INSERT INTO TutorialAppSchema.UserSalary (UserId"
                                    + ",Salary)"
                                    + "VALUES";
                    foreach (UserSalary singleUserSalary in userSalary)
                    {
                        string sqlToAdd = "(" + singleUserSalary.UserId
                                    + ", '" + singleUserSalary.Salary.ToString("0.00", CultureInfo.InvariantCulture)
                                    + "'),";
                        if ((sql + sqlToAdd).Length > 4000)
                        {
                            dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                            sql = "INSERT INTO TutorialAppSchema.UserSalary (UserId"
                                    + ",Salary)"
                                    + "VALUES";
                        }
                        sql += sqlToAdd;
                    }
                    dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                }
            }

            string userJobInfoJson = System.IO.File.ReadAllText("UserJobInfo.json");

            IEnumerable<UserJobInfo>? userJobInfo = JsonConvert.DeserializeObject<IEnumerable<UserJobInfo>>(userJobInfoJson);

            dataContextDapper.ExecuteSQL("TRUNCATE TABLE TutorialAppSchema.UserJobInfo");

            if (userJobInfo != null)
            {
                using (IDbConnection dbConnection = new SqlConnection(config.GetConnectionString("DefaultConnection")))
                {
                    string sql = "INSERT INTO TutorialAppSchema.UserJobInfo (UserId"
                                    + ",Department"
                                    + ",JobTitle)"
                                    + "VALUES";
                    foreach (UserJobInfo singleUserJobInfo in userJobInfo)
                    {
                        string sqlToAdd = "(" + singleUserJobInfo.UserId
                                    + ", '" + singleUserJobInfo.Department
                                    + "', '" + singleUserJobInfo.JobTitle
                                    + "'),";
                        if ((sql + sqlToAdd).Length > 4000)
                        {
                            dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                            sql = "INSERT INTO TutorialAppSchema.UserJobInfo (UserId"
                                    + ",Department"
                                    + ",JobTitle)"
                                    + "VALUES";
                        }
                        sql += sqlToAdd;
                    }
                    dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                }
            }
            Console.WriteLine("SQL Seed Completed Successfully");
        }
    }
}
