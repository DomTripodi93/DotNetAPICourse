using System;

namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string myFirstValue = "some words";
            string mySecondValue = "Some Words";
            //Write Your Code Here
            
            if (myFirstValue == mySecondValue)
            {
                Console.WriteLine("equal");
            }
            else if (myFirstValue.ToLower() == mySecondValue.ToLower())
            {
                Console.WriteLine("equal without case sensitivity");
            }
            else 
            {
                Console.WriteLine("not equal");
            }
        }
    }
}