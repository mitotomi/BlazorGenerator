using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                        w.WriteLine("\tpublic class " + table.dbTable + "Controller : Controller \n\t{");
                        w.WriteLine("\t\tRepo _repository=new Repo();\n\t\tRepo2 _repo2 = new Repo2();");
                        //get all
                        w.WriteLine("\t\t[HttpGet]\n\t\t[Route(\"api/" + table.dbTable.ToLower() + "s\")]");
                        w.WriteLine("\t\tpublic IEnumerable<" + table.dbTable + "> Get(){");
                        w.WriteLine("\t\t\treturn _repository.GetAll();\n\t\t}\n");
                        //get by id
                        w.WriteLine("\t\t[HttpGet]\n\t\t[Route(\"api/" + table.dbTable.ToLower() + "/{id}\")]");
                        w.WriteLine("\t\tpublic " + table.dbTable + " GetById(int id){");
                        w.WriteLine("\t\t\tif(id==0){\n\t\t\t\treturn new " + table.dbTable + "();\n\t\t\t}\n\t\t\telse{");
                        w.WriteLine("\t\t\t\treturn _repository.GetById(id);\n\t\t\t}\n\t\t}\n");
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

                        if (table.atributes.Where(x => x.foreignKey == true).Count() > 0)
                        {
                            Dictionary<string, List<string>> tableValuePairs = new Dictionary<string, List<string>>();
                            foreach (var attr in table.atributes.Where(x => x.foreignKey == true))
                            {
                                if (tableValuePairs.ContainsKey(attr.fkTable))
                                {
                                    tableValuePairs[attr.fkTable].Add(attr.fkValue);
                                }
                                else
                                {
                                    tableValuePairs[attr.fkTable] = new List<string>();
                                    tableValuePairs[attr.fkTable].Add(attr.fkValue);
                                }
                            }

                            foreach (var fkTable in tableValuePairs.Keys)
                            {
                                foreach (var value in tableValuePairs[fkTable])
                                {
                                    w.WriteLine("\t\t[HttpGet]\n\t\t[Route(\"api/" + table.dbTable.ToLower() + "s/" + fkTable.ToLower() + value.ToLower() + "\")]");
                                    w.WriteLine("\t\tpublic List<SelectListItem> Get" + fkTable + value + "SelectList(){");
                                    w.WriteLine("\t\t\tvar all=_repo2.GetAll();\n\t\t\tList<SelectListItem> options = new List<SelectListItem>();");
                                    w.WriteLine("\t\t\tforeach(var option in all){\n\t\t\t\toptions.Add(new SelectListItem(option.Id, option." + value + "));\n\t\t\t}");
                                    w.WriteLine("\t\t\treturn options;\n\t\t}");
                                }
                            }
                        }
                        foreach (var child in table.children)
                        {
                            w.WriteLine("\t\t[HttpGet]\n\t\t[Route(\"api/" + table.dbTable.ToLower() + "/" + child.dbTable.ToLower() + "/{id}\")]");
                            w.WriteLine("\t\tpublic List<" + child.dbTable + "> Get" + child.dbTable + "(int id){");
                            w.WriteLine("\t\t\treturn _repository.Get" + child.dbTable + "Children(id);\n\t\t}\n");
                        }
                        w.WriteLine("\t}\n}"); //closing for namespace and class
                    }
                }
            }
            foreach (NNModel nnModel in model.nnRelations)
            {
                using (FileStream fs = new FileStream(path + "\\" + nnModel.nnTable + "sController.cs", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("using " + projectName + ".Server.DataAccess;\nusing " + projectName + ".Shared.Models;\nusing Microsoft.AspNetCore.Mvc;\n" +
                            "using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Threading.Tasks;");
                        w.WriteLine();
                        w.WriteLine("namespace " + projectName + ".Server.Controllers \n{");
                        w.WriteLine("\tpublic class " + nnModel.nnTable + "Controller : Controller \n\t{");
                        w.WriteLine("\t\tRepo _repository=new Repo();\n\t\tRepo2 _repo2 = new Repo2();");
                        // get for first table
                        w.WriteLine("\t\t[HttpGet]\n\t\t[Route(\"api/" + nnModel.nnTable.ToLower() + "/" + nnModel.nnProps.table2.ToLower() + "/{id}\")]");
                        w.WriteLine("\t\tpublic List<" + nnModel.nnProps.table2 + "> GetFor" + nnModel.nnProps.table1 + " (int id){");
                        w.WriteLine("\t\t\treturn _repository.Get" + nnModel.nnProps.table2 + "(id);");
                        w.WriteLine("\t\t}\n");
                        // get for second table
                        w.WriteLine("\t\t[HttpGet]\n\t\t[Route(\"api/" + nnModel.nnTable.ToLower() + "/" + nnModel.nnProps.table1.ToLower() + "/{id}\")]");
                        w.WriteLine("\t\tpublic List<" + nnModel.nnProps.table1 + "> GetFor" + nnModel.nnProps.table2 + " (int id){");
                        w.WriteLine("\t\t\treturn _repository.Get" + nnModel.nnProps.table1 + "(id);");
                        w.WriteLine("\t\t}\n");
                        //get by ids
                        w.WriteLine("\t\t[HttpGet]\n\t\t[Route(\"api/" + nnModel.nnTable.ToLower() + "/{id1}/{id2}\")]");
                        w.WriteLine("\t\tpublic " + nnModel.nnTable + " GetById(int id1, int id2){");
                        w.WriteLine("\t\t\tif(id1==0 || id2==0){\n\t\t\t\tvar ret=new "+nnModel.nnTable+"();" +
                            "\n\t\t\t\tret."+nnModel.nnProps.attr1+"=id1;\n\t\t\t\tret."+nnModel.nnProps.attr2+"=id2;\n\t\t\t\treturn ret;\n\t\t\t}\n\t\t\telse{");
                        w.WriteLine("\t\t\t\treturn _repository.GetById(id1,id2);\n\t\t\t}\n\t\t}\n");
                        // post nn
                        w.WriteLine("\t\t[HttpPost]\n\t\t[Route(\"api/" + nnModel.nnTable.ToLower() + "/create\")]");
                        w.WriteLine("\t\tpublic void Post([FromBody] " + nnModel.nnTable + " model){");
                        w.WriteLine("\t\t\tif (ModelState.IsValid){\n\t\t\t\t_repository.Add(model);");
                        w.WriteLine("\t\t\t}\n\t\t}");
                        //update nn
                        w.WriteLine("\t\t[HttpPost]\n\t\t[Route(\"api/" + nnModel.nnTable.ToLower() + "/edit\")]");
                        w.WriteLine("\t\tpublic void Update([FromBody] " + nnModel.nnTable + " model){");
                        w.WriteLine("\t\t\tif (ModelState.IsValid) {\n\t\t\t\t_repository.Update(model);");
                        w.WriteLine("\t\t\t}\n\t\t}");
                        //delete nn
                        w.WriteLine("\t\t[HttpDelete]\n\t\t[Route(\"api/" + nnModel.nnTable.ToLower() + "/delete/{id1}/{id2}\")]");
                        w.WriteLine("\t\tpublic void Delete(int id1, int id2){");
                        w.WriteLine("\t\t\t_repository.Delete(id1, id2);");
                        w.WriteLine("\t\t}");

                        if (nnModel.atributes.Where(x => x.foreignKey == true).Count() > 0)
                        {
                            Dictionary<string, List<string>> tableValuePairs = new Dictionary<string, List<string>>();
                            foreach (var attr in nnModel.atributes.Where(x => x.foreignKey == true))
                            {
                                if (tableValuePairs.ContainsKey(attr.fkTable))
                                {
                                    tableValuePairs[attr.fkTable].Add(attr.fkValue);
                                }
                                else
                                {
                                    tableValuePairs[attr.fkTable] = new List<string>();
                                    tableValuePairs[attr.fkTable].Add(attr.fkValue);
                                }
                            }

                            foreach (var fkTable in tableValuePairs.Keys)
                            {
                                foreach (var value in tableValuePairs[fkTable])
                                {
                                    w.WriteLine("\t\t[HttpGet]\n\t\t[Route(\"api/" + nnModel.nnTable.ToLower() + "s/" + fkTable.ToLower() + value.ToLower() + "\")]");
                                    w.WriteLine("\t\tpublic List<SelectListItem> Get" + fkTable + value + "SelectList(){");
                                    w.WriteLine("\t\t\tvar all=_repo2.GetAll();\n\t\t\tList<SelectListItem> options = new List<SelectListItem>();");
                                    w.WriteLine("\t\t\tforeach(var option in all){\n\t\t\t\toptions.Add(new SelectListItem(option.Id, option." + value + "));\n\t\t\t}");
                                    w.WriteLine("\t\t\treturn options;\n\t\t}");
                                }
                            }
                        }

                        w.WriteLine("\t}\n}"); //closing for namespace and class
                    }
                }
            }
        }
    }
}
