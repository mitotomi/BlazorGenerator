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
                using (FileStream fs = new FileStream(path + "\\" + table.dbTable + "sController.cs", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("using " + projectName + ".Server.DataAccess;\nusing " + projectName + ".Shared.Models;\nusing Microsoft.AspNetCore.Mvc;\n" +
                            "using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Threading.Tasks;");
                        w.WriteLine();
                        w.WriteLine("namespace " + projectName + ".Server.Controllers \n{");
                        w.WriteLine("\tpublic class " + table.dbTable + "sController : Controller \n\t{");
                        w.WriteLine("\t\tRepo _repository=new Repo();");
                        //get all
                        w.WriteLine("\t\t[HttpGet]\n\t\t[Route(\"api/" + table.dbTable.ToLower() + "s\")]");
                        w.WriteLine("\t\tpublic IEnumerable<" + table.dbTable + "> Get(){");
                        w.WriteLine("\t\t\treturn _repository.GetAll();\n\t\t}\n");
                        //get by id
                        w.WriteLine("\t\t[HttpGet]\n\t\t[Route(\"api/" + table.dbTable.ToLower() + "/{id}\")]");
                        w.WriteLine("\t\tpublic IEnumerable<" + table.dbTable + "> GetById(int id){");
                        w.WriteLine("\t\t\treturn _repository.GetById(id);\n\t\t}\n");
                        //post
                        w.WriteLine("\t\t[HttpPost]\n\t\t[Route(\"api/" + table.dbTable.ToLower() + "/create\")]");
                        w.WriteLine("\t\tpublic void Post([FromBody] " + table.dbTable + " model){");
                        w.WriteLine("\t\t\tif (ModelState.IsValid){\n\t\t\t\t_repository.Add(model);");
                        w.WriteLine("\t\t\t}\n\t\t}");
                        //update
                        w.WriteLine("\t\t[HttpPost]\n\t\t[Route(\"api/" + table.dbTable.ToLower() + "/edit\")]");
                        w.WriteLine("\t\tpublic void Update([FromBody] " + table.dbTable + " model){");
                        w.WriteLine("\t\t\tif (ModelState.IsValid) {\n\t\t\t\t_repository.Update(model);");
                        w.WriteLine("\t\t\t}\n\t\t}");
                        //delete
                        w.WriteLine("\t\t[HttpDelete]\n\t\t[Route(\"api/" + table.dbTable.ToLower() + "/delete/{id}\")]");
                        w.WriteLine("\t\tpublic void Delete(int id){");
                        w.WriteLine("\t\t\t_repository.Delete(id);");
                        w.WriteLine("\t\t}");

                        w.WriteLine("\t}\n}"); //closing for namespace and class
                    }
                }
            }
        }
    }
}
