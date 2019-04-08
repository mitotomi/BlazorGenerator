using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ViewGenerator.Models;

namespace ViewGenerator.Generator
{
    public static class ControllerGenerator
    {
        public static void GenerateController(string path, TableModelCollection model, string projectName)
        {
            foreach (TableModel table in model.tableModels)
            {
                using (FileStream fs = new FileStream(path + "\\" + table.dbTable + "Table.cshtml", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("using "+projectName+".Server.DataAccess;\nusing Project.Shared.Models;\nusing Microsoft.AspNetCore.Mvc;\n" +
                            "using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Threading.Tasks;");
                        w.WriteLine();
                        w.WriteLine("namespace "+projectName+".Server.Controllers \n{");
                        w.WriteLine("public class " + table.dbTable + "sController : Controller \n\t{");

                        w.WriteLine("\t}\n}"); //closing for namespace and class
                    }
                }
            }
        }
    }
}
