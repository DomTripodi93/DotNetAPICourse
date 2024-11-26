using System;

namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Write Your Code Here
            
            Dictionary<string, decimal> itemPrices = new Dictionary<string, decimal>() {
                {"cheese", 5.99m},
                {"carrots", 2.99m}
            };
            
            //Write You Code Above This Line
            Console.WriteLine(itemPrices["cheese"].GetType());
            Console.WriteLine(itemPrices["cheese"]);
            Console.WriteLine(itemPrices["carrots"]);
        }
    }
}