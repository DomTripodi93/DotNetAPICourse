using System;
using System.Text.RegularExpressions;

namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> myNumberList = new List<int>(){
                2, 3, 5, 6, 7, 9, 10, 123, 324, 54
            };
            
            foreach (int number in myNumberList)
            {
                PrintIfOdd(number);
            }
        }
        //Write Your Code Here
        
        static void PrintIfOdd(int number)
        {
            if (number % 2 == 1)
            {
                Console.WriteLine(number);
            }
        }
    }
}