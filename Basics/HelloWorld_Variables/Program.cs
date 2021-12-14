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
            string myString = "new String";
            string stringWithSymbols = "!@#$%^&*()_'{}()`/\" giveitsomespace \\";
            // Console.WriteLine(stringWithSymbols.Length);

            char singleCharacter = '\"';

            bool myBool = false;

            sbyte mySbyte = 127;
            byte myByte = 255;
            short myShort = -32768;
            ushort myUshort = 65535;
            int myInt = -2147483648;
            long myLong = -9223372036854775808;


            float myFloat = 0.751f;
            float mySecondFloat = 0.75f;

            double myDouble = 0.751d;
            double mySecondDouble = 0.75;

            decimal myDecimal = 0.751m;
            decimal mySecondDecimal = 0.75m;

            Console.WriteLine(myFloat - mySecondFloat);
            Console.WriteLine(myDouble - mySecondDouble);
            Console.WriteLine(myDecimal - mySecondDecimal);

            string[] myGroceryArray = new string[2];
            myGroceryArray[0] = "Guacamole";
            myGroceryArray[1] = "Ice Cream";

            Console.WriteLine(myGroceryArray[0]);

            string[] mySecondGroceryArray = { "Apples", "Eggs" };

            mySecondGroceryArray[0] = "Oranges";

            Console.WriteLine(mySecondGroceryArray[0]);

            List<string> myGroceryList = new List<string>() { "Milk", "Cheese" };

            myGroceryList.Add("Ham");

            // Console.WriteLine(myStringList[2]);

            IEnumerable<string> myGroceryIEnumerable = myGroceryList;
            IEnumerable<string> mySecondGroceryIEnumerable = myGroceryList;

            Console.WriteLine(myGroceryIEnumerable.First());

            string[,] myTwoDArray = {
                {"Guacamole", "Ice Cream", "Apples"},
                {"Milk", "Cheese", "Eggs"}
            };

            Console.WriteLine(myTwoDArray[0, 1]);

            string[,,] myThreeDArray = {
                {
                    {"Guacamole", "Ice Cream", "Apples"},
                    {"Milk", "Cheese", "Eggs"}
                },
                {
                    {"Guacamole", "Ice Cream", "Apples"},
                    {"Milk", "Cheese", "Eggs"}
                }
            };

            Console.WriteLine(myThreeDArray[0, 1, 1]);


            Dictionary<string, string> car = new Dictionary<string, string>(){
                {"transmission", "automatic"}
            };

            Console.WriteLine(car["transmission"]);

            car["make"] = "Honda";

            Console.WriteLine(car["make"]);


            Dictionary<string, string[]> myGroceriesByDepartment = new Dictionary<string, string[]>{
                {"Dairy", new string[]{"Milk", "Cheese", "Eggs"}},
                {"Meat", new string[]{"Chicken", "Beef", "Pork"}},
                {"Produce", new string[]{"Apples", "Oranges", "Spinach", "Broccoli"}}
            };

        }
    }
}
