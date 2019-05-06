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
            string projectName = "Master_v2";

            var model = SetupReader.ReadData(jsonPath);
            //PageGenerator.GenerateReadView(destPath+"Views", model, projectName);
            //PageGenerator.GenerateTableView(destPath+"Views", model, projectName);
            //PageGenerator.GenerateCreateUpdate(destPath+"Views", model, projectName);
            //PageGenerator.GenerateDelete(destPath+"Views",model,projectName);
            //ControllerGenerator.GenerateController(destPath+"Controllers", model, projectName);
            //DataAccessLayerGenerator.GenerateDataAccessLayer(destPath + "DataAccess", model, projectName);


            Console.WriteLine("finished");
            Console.ReadKey();
        }
    }
}
