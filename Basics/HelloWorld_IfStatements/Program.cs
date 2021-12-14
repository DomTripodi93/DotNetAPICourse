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
            int smallerInt = 17;
            int largerInt = 25;


            if (smallerInt > largerInt)
            {
                smallerInt += 10;
            }

            Console.WriteLine(smallerInt);

            if (smallerInt < largerInt)
            {
                smallerInt += 20;
            }

            Console.WriteLine(smallerInt);

            string firstStringToCompare = "Given Value";

            string secondStringToCompare = "given value";

            if (firstStringToCompare == secondStringToCompare)
            {
                Console.WriteLine("Same");
            }

            if (firstStringToCompare != secondStringToCompare)
            {
                Console.WriteLine("Different");
            }

            if (firstStringToCompare.ToLower() != secondStringToCompare.ToLower())
            {
                Console.WriteLine("Same without case sensitivity");
            }

            string partialStringToCompare = "Value";

            if (firstStringToCompare.Contains(partialStringToCompare))
            {
                Console.WriteLine("Contained");
            }

            if (secondStringToCompare.Contains(partialStringToCompare))
            {
                Console.WriteLine("Contained");
            }

            if (secondStringToCompare.Contains(partialStringToCompare.ToLower()))
            {
                Console.WriteLine("Contained");
            }

            if (firstStringToCompare.Contains(partialStringToCompare) && secondStringToCompare.Contains(partialStringToCompare))
            {
                Console.WriteLine("Contained");
            }

            if (firstStringToCompare.Contains(partialStringToCompare) || secondStringToCompare.Contains(partialStringToCompare))
            {
                Console.WriteLine("Contained");
            }

            string stringCaseValue = "Switch Value";
            
            switch (stringCaseValue)
            {
                case "not my switch value":
                    Console.WriteLine("First case");
                    break;
                case "Switch Value":
                // case "switch value":
                    Console.WriteLine("Second case");
                    break;
                default:
                    Console.WriteLine("No set case");
                    break;
            }

            int firstCaseInt = 12;
            // int firstCaseInt = -1;
            int secondCaseInt = 16;
            
            switch (firstCaseInt)
            {
                case >15:
                    Console.WriteLine("Greater than 15");
                    break;
                case <0:
                    Console.WriteLine("less than 0");
                    break;
                default:
                    Console.WriteLine("No set case");
                    break;
            }
            
            switch ((firstCaseInt, secondCaseInt))
            {
                case (>20, <20):
                    Console.WriteLine("First numeric case");
                    break;
                case (<13, <20):
                    Console.WriteLine("Second numeric case");
                    break;
                default:
                    Console.WriteLine("No set case");
                    break;
            }
                

        }
    }
}
