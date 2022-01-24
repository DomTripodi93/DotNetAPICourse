using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.Extensions.Configuration;

namespace HelloWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {

            IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json")
                    .Build();

            Computer myComputer = new Computer();
            myComputer.Motherboard = "Z690";
            myComputer.CPUCores = 4;
            myComputer.HasWifi = true;
            myComputer.HasLTE = false;
            myComputer.ReleaseDate = DateTime.Today;
            myComputer.Price = 859.95m;
            myComputer.VideoCard = "rtx 2060";
            
            string sql = @"INSERT INTO TutorialAppSchema.Computer (Motherboard
                                    , CPUCores
                                    , HasWifi
                                    , HasLTE
                                    , ReleaseDate
                                    , Price
                                    , VideoCard)
                            VALUES ('" + myComputer.Motherboard 
                            + "', " + myComputer.CPUCores 
                            + ", '" + myComputer.HasWifi 
                            + "', '" + myComputer.HasLTE 
                            + "', '" + myComputer.ReleaseDate.ToString("yyyy-MM-dd")
                            + "', " + myComputer.Price.ToString() 
                            + ", '" + myComputer.VideoCard
                            + "')";

            // File.WriteAllText("Log.txt", sql);

            using StreamWriter openFile = new("Log.txt", append: true);

            openFile.WriteLine(sql);

            openFile.Close();

            string fileRead = File.ReadAllText("Log.txt");

            Console.WriteLine(fileRead);

            DataContextDapper dataContextDapper = new DataContextDapper(config);

            dataContextDapper.ExecuteSQL("TRUNCATE TABLE TutorialAppSchema.Computer");

            dataContextDapper.ExecuteSQL(sql);

            DataContextEF dataContextEF = new DataContextEF(config);

            dataContextDapper.ExecuteSQL("TRUNCATE TABLE TutorialAppSchema.ComputerForTestApp");

            dataContextEF.Add(myComputer);
            dataContextEF.SaveChanges();

            IEnumerable<Computer> computersFromDataBaseDapper = dataContextDapper.LoadData<Computer>("SELECT * FROM TutorialAppSchema.Computer");
            foreach (Computer singleComputerFromDataBaseDapper in computersFromDataBaseDapper)
            {
                Console.WriteLine("ComputerId: " + singleComputerFromDataBaseDapper.ComputerId);
                Console.WriteLine("Motherboard: " + singleComputerFromDataBaseDapper.Motherboard);
                Console.WriteLine("CPUCores: " + singleComputerFromDataBaseDapper.CPUCores);
                Console.WriteLine("HasWifi: " + singleComputerFromDataBaseDapper.HasWifi);
                Console.WriteLine("HasLTE: " + singleComputerFromDataBaseDapper.HasLTE);
                Console.WriteLine("ReleaseDate: " + singleComputerFromDataBaseDapper.ReleaseDate.ToString("yyyy-MM-dd"));
                Console.WriteLine("Price: " + singleComputerFromDataBaseDapper.Price.ToString());
                Console.WriteLine("VideoCard: " + singleComputerFromDataBaseDapper.VideoCard);
                Console.WriteLine("");
            }

            IEnumerable<Computer>? computersFromDataBaseEF = dataContextEF.Computer?.ToList<Computer>();
            if (computersFromDataBaseEF != null)
            {
                foreach (Computer singleComputerFromDataBaseEF in computersFromDataBaseEF)
                {
                    Console.WriteLine("ComputerId: " + singleComputerFromDataBaseEF.ComputerId);
                    Console.WriteLine("Motherboard: " + singleComputerFromDataBaseEF.Motherboard);
                    Console.WriteLine("CPUCores: " + singleComputerFromDataBaseEF.CPUCores);
                    Console.WriteLine("HasWifi: " + singleComputerFromDataBaseEF.HasWifi);
                    Console.WriteLine("HasLTE: " + singleComputerFromDataBaseEF.HasLTE);
                    Console.WriteLine("ReleaseDate: " + singleComputerFromDataBaseEF.ReleaseDate.ToString("yyyy-MM-dd"));
                    Console.WriteLine("Price: " + singleComputerFromDataBaseEF.Price.ToString());
                    Console.WriteLine("VideoCard: " + singleComputerFromDataBaseEF.VideoCard);
                    Console.WriteLine("");
                }
            }
        }
    }
}
