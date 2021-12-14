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
            int[] intsToCompress = {10, 15, 20, 25, 30, 35, 40, 45};

            int totalValue = intsToCompress[0] + intsToCompress[1] 
                + intsToCompress[2] + intsToCompress[3] 
                + intsToCompress[4] + intsToCompress[5] 
                + intsToCompress[6] + intsToCompress[7];

            Console.WriteLine(totalValue);
            
            
            int totalValueTwo = 0;
            
            // for (int i = 0; i < 7; i++)
            for (int i = 0; i < intsToCompress.Length; i++)
            {
                totalValueTwo += intsToCompress[i];
            }

            Console.WriteLine(totalValueTwo);


            int totalValueThree = 0;

            foreach(int intToAdd in intsToCompress)
            {
                totalValueThree += intToAdd;
            }

            Console.WriteLine(totalValueThree);


            int totalValueFour = intsToCompress.Sum();

            Console.WriteLine(totalValueFour);


            string[] stringsToCheck = {};
            // string[] stringsToCheck = {"iterated value"};

            int iteration = 0;

            do
            {
                Console.WriteLine("Do While Ran");
                // Console.WriteLine(stringsToCheck[iteration]);
                iteration++;
            } while (iteration < stringsToCheck.Length);


            while (iteration < stringsToCheck.Length)
            {
                Console.WriteLine("While Ran");
                Console.WriteLine(stringsToCheck[iteration]);
                iteration++;
            };
        }
    }
}
