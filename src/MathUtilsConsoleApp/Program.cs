using System;

namespace MathUtilsConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{i} is even? {IsEven(i)}");
            }
        }

        static bool IsEven(int i) => DotNot.Math.MathUtils.IsEven(i);
    }
}
