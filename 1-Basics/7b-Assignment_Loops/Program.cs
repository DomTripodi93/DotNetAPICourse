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
            //Write Your Code Here
            
            foreach (int number in myNumberList)
            {
                if (number % 2 == 0)
                {
                    Console.WriteLine(number);
                }
            }
        }
    }
}