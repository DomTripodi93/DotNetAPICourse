using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace HelloWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Computer myComputer = new Computer();
            Computer myComputer = new Computer(
                "Z690",
                4,
                true,
                false,
                DateTime.Today,
                859.95m,
                "RTX 2060"
            );

            Console.WriteLine(myComputer.Price);
        }
    }

    public partial class Computer
    {
        public string? Motherboard { get; set; }
        public int CPUCores { get; set; }
        public bool HasWifi { get; set; }
        public bool HasLTE { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public string? VideoCard { get; set; }
        public Computer(string motherboard, int cpuCores, bool hasWifi, bool hasLTE, DateTime releaseDate, decimal price, string videoCard)
        {
            Motherboard = motherboard;
            CPUCores = cpuCores;
            HasWifi = hasWifi;
            HasLTE = hasLTE;
            ReleaseDate = releaseDate;
            Price = price;
            VideoCard = videoCard;
        }
    }

    public partial class Computer
    {
        // public string? VideoCard { get; set; }
    }
}
