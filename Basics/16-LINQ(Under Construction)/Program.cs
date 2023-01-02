using System;
using System.Data;
using System.Text.Json;
using System.Text.RegularExpressions;
using AutoMapper;
using Dapper;
using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DataContextDapper dapper = new DataContextDapper(config);

            string computersJson = File.ReadAllText("ComputersSnake.json");

            Mapper mapper = new Mapper(new MapperConfiguration((cfg) => {
                cfg.CreateMap<ComputerSnake, Computer>()
                    .ForMember(destination => destination.ComputerId, options => 
                        options.MapFrom(source => source.computer_id))
                    .ForMember(destination => destination.CPUCores, options => 
                        options.MapFrom(source => source.cpu_cores))
                    .ForMember(destination => destination.HasLTE, options => 
                        options.MapFrom(source => source.has_lte))
                    .ForMember(destination => destination.HasWifi, options => 
                        options.MapFrom(source => source.has_wifi))
                    .ForMember(destination => destination.Motherboard, options => 
                        options.MapFrom(source => source.motherboard))
                    .ForMember(destination => destination.VideoCard, options => 
                        options.MapFrom(source => source.video_card))
                    .ForMember(destination => destination.ReleaseDate, options => 
                        options.MapFrom(source => source.release_date))
                    .ForMember(destination => destination.Price, options => 
                        options.MapFrom(source => source.price));
            }));
            
            // IEnumerable<ComputerSnake>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ComputerSnake>>(computersJson);

            // if (computersSystem != null)
            // {
            //     IEnumerable<Computer> computerResult = mapper.Map<IEnumerable<Computer>>(computersSystem);
            //     Console.WriteLine("Automapper Count: " +  computerResult.Count());
            // }

            IEnumerable<Computer>? computersJsonPropertyMapping = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson);
            if (computersJsonPropertyMapping != null)
            {
                Console.WriteLine("JSON Property Count: " + computersJsonPropertyMapping.Count());


                IEnumerable<Computer> computersWithWifi = computersJsonPropertyMapping.Where(c => c.HasWifi).ToList();

                IEnumerable<Computer> computersWithLTE = computersJsonPropertyMapping.Where(c => c.HasLTE).ToList();

                IEnumerable<Computer> computersWithLTEAndWifiSimple = computersWithWifi.Where(c => !c.HasLTE).ToList();
                IEnumerable<Computer> computersWithLTEAndWifiAny = computersWithWifi.Where(c => !computersWithLTE.Any(c2 => c2.ComputerId == c.ComputerId )).ToList();
                IEnumerable<Computer> computersWithLTEAndWifiAll = computersWithWifi.Where(c => computersWithLTE.All(c2 => c2.ComputerId != c.ComputerId )).ToList();

                IEnumerable<int> computerIds = 
                    from computer in computersJsonPropertyMapping 
                    where computer.ComputerId > 50 
                    orderby computer.ComputerId descending//ascending
                    select computer.ComputerId;
                

                Console.WriteLine("Filtered Results Count: " + computersWithWifi.Count());
                foreach(Computer computerWithWifi in computersWithWifi)
                {
                    Console.WriteLine("ComputerId " + computerWithWifi.ComputerId.ToString() + ", Motherboard " + computerWithWifi.Motherboard);
                }

            }

        }

        static string EscapeSingleQuote(string input)
        {
            string output = input.Replace("'", "''");

            return output;
        }

    }
}