using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelloWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {

            IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json")
                    .Build();

            string computersJson = System.IO.File.ReadAllText("Computers.json");

            IEnumerable<Computer>? computers = JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson);

            string serializedComputers = JsonSerializer.Serialize(computers);

            using StreamWriter openFile = new("ComputersCopy.json", append: true);

            openFile.WriteLine(serializedComputers);

            openFile.Close();

            DataContextDapper dataContextDapper = new DataContextDapper(config);

            dataContextDapper.ExecuteSQL("TRUNCATE TABLE TutorialAppSchema.Computer");

            if (computers != null)
            {
                using (IDbConnection dbConnection = new SqlConnection(config.GetConnectionString("DefaultConnection")))
                {
                    foreach (Computer singleComputer in computers)
                    {
                        string sql = @"INSERT INTO TutorialAppSchema.Computer (Motherboard
                                            , CPUCores
                                            , HasWifi
                                            , HasLTE
                                            , ReleaseDate
                                            , Price
                                            , VideoCard)
                                VALUES ('" + singleComputer.Motherboard?.Replace("'", "''")
                                    + "', " + singleComputer.CPUCores
                                    + ", '" + singleComputer.HasWifi
                                    + "', '" + singleComputer.HasLTE
                                    + "', '" + singleComputer.ReleaseDate?.ToString("yyyy-MM-dd")
                                    + "', " + singleComputer.Price.ToString()
                                    + ", '" + singleComputer.VideoCard?.Replace("'", "''")
                                    + "')";

                        dataContextDapper.ExecuteSqlMulti(sql, dbConnection);
                    }
                }
            }

            IEnumerable<Computer> computersFromDataBaseDapper = dataContextDapper.LoadData<Computer>("SELECT * FROM TutorialAppSchema.Computer");
            foreach (Computer singleComputerFromDataBaseDapper in computersFromDataBaseDapper)
            {
                Console.WriteLine("ComputerId: " + singleComputerFromDataBaseDapper.ComputerId);
                Console.WriteLine("Motherboard: " + singleComputerFromDataBaseDapper.Motherboard);
                Console.WriteLine("CPUCores: " + singleComputerFromDataBaseDapper.CPUCores);
                Console.WriteLine("HasWifi: " + singleComputerFromDataBaseDapper.HasWifi);
                Console.WriteLine("HasLTE: " + singleComputerFromDataBaseDapper.HasLTE);
                Console.WriteLine("ReleaseDate: " + singleComputerFromDataBaseDapper.ReleaseDate?.ToString("yyyy-MM-dd"));
                Console.WriteLine("Price: " + singleComputerFromDataBaseDapper.Price.ToString());
                Console.WriteLine("VideoCard: " + singleComputerFromDataBaseDapper.VideoCard);
                Console.WriteLine("");
            }
        }
    }
}
