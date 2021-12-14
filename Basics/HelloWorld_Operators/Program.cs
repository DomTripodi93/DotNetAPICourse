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

            Console.WriteLine(mySbyte + myDecimal);

            int mySecondInt = 5;
            int myThirdInt = 10;

            Console.WriteLine(mySecondInt * myThirdInt);
            Console.WriteLine(myThirdInt / mySecondInt);

            Console.WriteLine(5 + 5 * 10);
            Console.WriteLine((5 + 5) * 10);

            Console.WriteLine(mySecondInt);

            mySecondInt++;

            Console.WriteLine(mySecondInt);

            mySecondInt += 3;

            Console.WriteLine(mySecondInt);

            Console.WriteLine(Math.Pow(3, 2));
            Console.WriteLine(Math.Sqrt(9));

            Console.WriteLine(mySecondInt.Equals(myThirdInt));
            Console.WriteLine(mySecondInt == myThirdInt);

            mySecondInt++;

            Console.WriteLine(mySecondInt.Equals(myThirdInt));
            Console.WriteLine(mySecondInt == myThirdInt);
            Console.WriteLine(mySecondInt != myThirdInt);
            Console.WriteLine(mySecondInt > myThirdInt);
            Console.WriteLine(mySecondInt >= myThirdInt);
            Console.WriteLine(mySecondInt < myThirdInt);
            Console.WriteLine(mySecondInt <= myThirdInt);

            Console.WriteLine(mySecondInt == myThirdInt & mySecondInt <= myThirdInt);
            Console.WriteLine(mySecondInt < myThirdInt && mySecondInt <= myThirdInt); //short-cycling

            Console.WriteLine(mySecondInt != myThirdInt | mySecondInt < myThirdInt);
            Console.WriteLine(mySecondInt == myThirdInt || mySecondInt < myThirdInt); //short-cycling
            
        }
    }
}
