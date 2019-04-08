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
            string destPath = @"C:\Users\Tomislav\Documents\Visual Studio 2017\Projects\Master_v2\ViewGenerator\";
            string ProjectName = "Master_v2";

            var model = SetupReader.ReadData(jsonPath);
            PageGenerator.GenerateReadView(destPath+"Views", model, ProjectName);
            PageGenerator.GenerateTableView(destPath+"Views", model, ProjectName);
            ControllerGenerator.GenerateController(destPath+"Controllers", model, ProjectName);

            Console.WriteLine("finished");
            Console.ReadKey();
        }
    }
}
