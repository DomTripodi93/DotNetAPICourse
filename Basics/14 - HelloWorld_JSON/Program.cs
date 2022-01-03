using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HelloWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {

            IConfiguration Config = new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json")
                    .Build();

            string computersJson = System.IO.File.ReadAllText("Computers.json");

            IEnumerable<Computer>? computers = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);

            string serializedComputers = JsonConvert.SerializeObject(computers);

            using StreamWriter openFile = new("ComputersCopy.json", append: true);

            openFile.WriteLine(serializedComputers);

            openFile.Close();

            DataContextDapper dataContextDapper = new DataContextDapper(Config);

            dataContextDapper.ExecuteSQL("TRUNCATE TABLE TestAppSchema.Computer");
            
            if (computers != null)
            {
                foreach (Computer singleComputer in computers)
                {
                    string sql = @"INSERT INTO TestAppSchema.Computer (Motherboard
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

                    dataContextDapper.ExecuteSQL(sql);
                }
            }

            IEnumerable<Computer> computersFromDataBaseDapper = dataContextDapper.LoadData<Computer>("SELECT * FROM TestAppSchema.Computer");
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
