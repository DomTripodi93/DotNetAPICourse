using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using HelloWorld.Models;

namespace HelloWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
}
