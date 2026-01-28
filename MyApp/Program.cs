using System;
using DemoDLL;
namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world!");
            var helper = new Class1();

            Console.WriteLine("Sum: " + helper.Add(5, 7));
            Console.WriteLine("Product: " + helper.Multiply(3, 4));

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
