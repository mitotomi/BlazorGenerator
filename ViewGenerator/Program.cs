using System;
using System.IO;
using ViewGenerator.Generator;

namespace ViewGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonPath = @"C:\Users\Tomislav\Documents\Visual Studio 2017\Projects\Master_v2\Master_v2.Server\generatorConfig.json";
            string viewsPath = @"C:\Users\Tomislav\Documents\Visual Studio 2017\Projects\Master_v2\ViewGenerator\Views";
            var model = SetupReader.ReadData(jsonPath);
            
            PageGenerator.GenerateReadView(viewsPath, model);
            PageGenerator.GenerateTableView(viewsPath, model);
            Console.WriteLine("finished");
            Console.ReadKey();
        }
    }
}
