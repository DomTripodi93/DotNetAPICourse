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
            // Computer myComputer = new Computer(
            //     "Z690",
            //     4,
            //     true,
            //     false,
            //     DateTime.Today,
            //     859.95m,
            //     "RTX 2060"
            // );
            Computer myComputer = new Computer();
            // Computer myComputer = new Computer(
            //     "Z690",
            //     4,
            //     true,
            //     false,
            //     DateTime.Today,
            //     859.95m,
            //     "RTX 2060"
            // );
            myComputer.Motherboard = "Z690";
            myComputer.CPUCores = 4;
            myComputer.HasWifi = true;
            myComputer.HasLTE = false;
            myComputer.ReleaseDate = DateTime.Today;
            myComputer.Price = 859.95m;
            myComputer.VideoCard = "rtx 2060";

            Console.WriteLine(myComputer.Price);
            string sql = @"INSERT INTO TestAppSchema.Computer (Motherboard
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
            Console.WriteLine(sql);

            DataContextDapper dataContextDapper = new DataContextDapper();

            dataContextDapper.ExecuteSQL("TRUNCATE TABLE TestAppSchema.Computer");

            dataContextDapper.ExecuteSQL(sql);

            DataContextEF dataContextEF = new DataContextEF();

            dataContextDapper.ExecuteSQL("TRUNCATE TABLE TestAppSchema.ComputerForTestApp");

            dataContextEF.Add(myComputer);
            dataContextEF.SaveChanges();

            IEnumerable<Computer> computersFromDataBaseDapper = dataContextDapper.LoadData<Computer>("SELECT * FROM TestAppSchema.Computer");
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
