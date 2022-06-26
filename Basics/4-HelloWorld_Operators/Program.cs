using System;

namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int myInt = 5;
            int mySecondInt = 10;

            Console.WriteLine(myInt.Equals(mySecondInt));
            Console.WriteLine(myInt.Equals(mySecondInt / 2));

            Console.WriteLine(myInt != mySecondInt);
            Console.WriteLine(myInt == mySecondInt / 2);
            Console.WriteLine(myInt != mySecondInt / 3);

            Console.WriteLine(myInt >= mySecondInt);
            Console.WriteLine(myInt >= mySecondInt / 2);
            Console.WriteLine(myInt > mySecondInt);
            Console.WriteLine(myInt > mySecondInt - 6);
            Console.WriteLine(myInt <= mySecondInt);
            Console.WriteLine(myInt < mySecondInt);

            Console.WriteLine(5 > 10 && 5 < 10);


            //Mathematical Operators


            Console.WriteLine(myInt);

            myInt++;

            Console.WriteLine(myInt);

            myInt += 7;

            Console.WriteLine(myInt);

            myInt -= 8;

            Console.WriteLine(myInt);

            Console.WriteLine(myInt * mySecondInt);

            Console.WriteLine(mySecondInt / myInt);

            Console.WriteLine(mySecondInt + myInt);

            Console.WriteLine(myInt - mySecondInt);

            Console.WriteLine(5 + 5 * 10);

            Console.WriteLine((5 + 5) * 10);

            Console.WriteLine(Math.Pow(5, 4));

            Console.WriteLine(Math.Sqrt(25));

            string myString = "test";

            Console.WriteLine(myString);

            myString += ". second part.";

            Console.WriteLine(myString);

            myString = myString + " \"third\\ part.";

            Console.WriteLine(myString);

            string[] myStringArr = myString.Split(". ");
            Console.WriteLine(myStringArr[0]);
            Console.WriteLine(myStringArr[1]);




        }
    }
}