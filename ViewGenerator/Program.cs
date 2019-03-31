using System;

namespace ViewGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SetupReader reader = new SetupReader();
            reader.readData();
            Console.ReadKey();
        }
    }
}
