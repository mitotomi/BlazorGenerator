using System;
using System.IO;

namespace ViewGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonPath = @"C:\Users\Tomislav\Documents\Visual Studio 2017\Projects\Master_v2\Master_v2.Server\generatorConfig.json";
            string viewsPath = @"C:\Users\Tomislav\Documents\Visual Studio 2017\Projects\Master_v2\ViewGenerator\Views";
            var model = SetupReader.ReadData(jsonPath);
            Generator.GenerateView(viewsPath, model);
            Console.ReadKey();
        }
    }
}
