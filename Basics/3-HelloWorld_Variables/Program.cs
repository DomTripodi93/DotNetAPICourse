using System;

namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            sbyte mySbyte = 127;
            sbyte mySecondSbyte = -128;
            byte myByte = 255;
            byte mySecondByte = 0;

            short myShort = -32768;
            ushort myUshort = 65535;

            int myInt = 2147483647;
            int mySecondInt = -2147483648;

            long myLong = -9223372036854775808;


            float myFloat = 0.751f;
            float mySecondFloat = 0.75f;

            double myDouble = 0.751;
            double mySecondDouble = 0.75d;

            decimal myDecimal = 0.751m;
            decimal mySecondDecimal = 0.75m;

            // Console.WriteLine(myFloat - mySecondFloat);
            // Console.WriteLine(myDouble - mySecondDouble);
            // Console.WriteLine(myDecimal - mySecondDecimal);

            string myString = "Hello World";
            // Console.WriteLine(myString);
            string myStringWithSymbols = "!@#$@^$%%^&(&%^*__)+%^@##$!@%123589071340698ughedfaoig1378904cbt90fty89701c2byt34";
            // Console.WriteLine(myStringWithSymbols);

            bool myBool = true;

            string[] myGroceryArray = new string[2];

            myGroceryArray[0] = "Guacamole";
            // myGroceryArray[2] = "Ice Cream";

            // Console.WriteLine(myGroceryArray[0]);
            // Console.WriteLine(myGroceryArray[1]);
            // Console.WriteLine(myGroceryArray[2]);

            string[] mySecondGroceryArray = { "Apples", "Eggs" };

            // mySecondGroceryArray[2] = ""; //Error

            // Console.WriteLine(mySecondGroceryArray[0]);
            // Console.WriteLine(mySecondGroceryArray[1]);
            // Console.WriteLine(mySecondGroceryArray[2]); //Error

            List<string> myGroceryList = new List<string>() { "Milk", "Cheese" };

            // Console.WriteLine(myGroceryList[0]);
            // Console.WriteLine(myGroceryList[1]);

            // Console.WriteLine(myGroceryList[2]); //Error

            myGroceryList.Add("Oranges");

            // Console.WriteLine(myGroceryList[2]);

            IEnumerable<string> myGroceryIEnumerable =  myGroceryList;

            // Console.WriteLine(myGroceryIEnumerable.First());

            string[,] myTwoDimensionalArray = new string[,] {
                { "Apples", "Eggs" },
                { "Milk", "Cheese" }
            };

            // Console.WriteLine(myTwoDimensionalArray[1,1]);

            Dictionary<string, string[]> myGroceryDictionary = new Dictionary<string, string[]>(){
                {"Dairy", new string[]{"Cheese", "Milk", "Eggs"}}
            };

            Console.WriteLine(myGroceryDictionary["Dairy"][2]);
        }
    }
}