using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AutoMapper;
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

            IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appSettings.json")
                    .Build();

            string computersJson = System.IO.File.ReadAllText("Computers.json");

            IEnumerable<ComputerSnakeCase>? computersSnakeCase = JsonConvert.DeserializeObject<IEnumerable<ComputerSnakeCase>>(computersJson);

            
            Mapper mapper = new Mapper(new MapperConfiguration(cfg => { 
                cfg.CreateMap<ComputerSnakeCase, Computer>()
                    .ForMember(destination => destination.ComputerId, options =>
                        options.MapFrom(source => source.computer_id))
                    .ForMember(destination => destination.Motherboard, options =>
                        options.MapFrom(source => source.motherboard))
                    .ForMember(destination => destination.HasWifi, options =>
                        options.MapFrom(source => source.has_wifi))
                    .ForMember(destination => destination.HasLTE, options =>
                        options.MapFrom(source => source.has_lte))
                    .ForMember(destination => destination.ReleaseDate, options =>
                        options.MapFrom(source => source.release_date))
                    .ForMember(destination => destination.VideoCard, options =>
                        options.MapFrom(source => source.video_card));
            }));

            IEnumerable<Computer> computers = mapper.Map<IEnumerable<Computer>>(computersSnakeCase);

            DataContextDapper dataContextDapper = new DataContextDapper(config);

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
