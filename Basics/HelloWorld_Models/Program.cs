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
            Computer myComputer = new Computer();
            myComputer.Motherboard = "Z690";
            myComputer.CPUCores = 4;
            myComputer.HasWifi = true;
            myComputer.HasLTE = false;
            myComputer.ReleaseDate = DateTime.Today;
            myComputer.Price = 859.95m;
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
    }
}
