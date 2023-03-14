using System;

namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Write Your Code Here
            
            string myString = "";
            decimal myDecimal = 1.5m;
            bool myBoolean = true;
            
            //Write You Code Above This Line
            Console.WriteLine(myString.GetType());
            Console.WriteLine(myDecimal.GetType());
            Console.WriteLine(myBoolean.GetType());
        }
    }
}