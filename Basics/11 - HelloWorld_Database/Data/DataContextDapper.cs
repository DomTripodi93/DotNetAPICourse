using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HelloWorld.Data
{
    public class DataContextDapper
    {
        public IEnumerable<T> LoadData<T>(string sql)
        {
            // using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            using (IDbConnection dbConnection = new SqlConnection("Server=localhost;Database=DotNetCourseDatabase;Trusted_Connection=true;TrustServerCertificate=true;"))
            {
                dbConnection.Open();
                using (IDbTransaction tran = dbConnection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var holdVal = dbConnection.Query<T>(sql, null, transaction: tran, commandTimeout: 999999999);
                    dbConnection.Close();
                    return holdVal;
                }
            }
        }

        public int ExecuteSQL(string sql)
        {
            using (IDbConnection dbConnection = new SqlConnection("Server=localhost;Database=DotNetCourseDatabase;Trusted_Connection=true;TrustServerCertificate=true;"))
            {
                return dbConnection.Execute(sql);
            }
        }

    }
}
