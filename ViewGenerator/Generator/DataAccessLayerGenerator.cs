using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ViewGenerator.Models;

namespace ViewGenerator.Generator
{
    public static class DataAccessLayerGenerator
    {
        public static void GenerateDataAccessLayer(string path, TableModelCollection model, string project)
        {
            foreach (TableModel table in model.tableModels)
            {
                using (FileStream fs = new FileStream(path + "\\" + table.dbTable + "AccessLayer.cs", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("using System;\nusing "+project+".Server.Models;\n"+"using "+project+".Shared.Models;\nusing System.Collections.Generic;\n");
                        w.WriteLine("using System.Linq;\nusing Microsoft.EntityFrameworkCore;");
                        w.WriteLine("namespace " + project + ".Server.DataAccess\n{");
                        w.WriteLine("\tpublic class " + table.dbTable + "AccessLayer\n{\n");
                        w.WriteLine("\t\tContext _context = new Context();\n");
                        //get all
                        w.WriteLine("\t\tpublic IEnumerable<"+table.dbTable+"> GetAll()\n\t\t{");
                        w.WriteLine("\t\t\ttry{\n\t\t\t\treturn _context."+table.dbTable+";\n\t\t\t}");
                        w.WriteLine("\t\t\tcatch(Exception e){\n\t\t\t\treturn null;\n\t\t\t}\n\t\t}\n");
                        // post
                        w.WriteLine("\t\tpublic void Add("+table.dbTable+" "+table.dbTable.ToLower()+"){");
                        w.WriteLine("\t\t\ttry{\n\t\t\t\t_context."+table.dbTable+".Add("+table.dbTable.ToLower()+");");
                        w.WriteLine("\t\t\t\t_context.SaveChanges();\n\t\t\t}");
                        w.WriteLine("\t\t\tcatch(Exception e){\n\n\t\t\t}\n\t\t}\n\n");
                        //update
                        w.WriteLine("\t\tpublic void Update("+table.dbTable+" "+table.dbTable.ToLower()+"){");
                        w.WriteLine("\t\t\ttry{\n\t\t\t\tif("+table.dbTable.ToLower()+" != null){\n\t\t\t\t\t_context."+table.dbTable+".Update("+table.dbTable.ToLower()+");");
                        w.WriteLine("\t\t\t\t\t_context.SaveChanges();\n\t\t\t\t}\n\t\t\t}");
                        w.WriteLine("\t\t\tcatch(Exception e){\n\n\t\t\t}\n\t\t}\n\n");
                        // get by id
                        w.WriteLine("\t\tpublic " + table.dbTable + " GetById(int id){");
                        w.WriteLine("\t\t\ttry{\n\t\t\t\treturn _context." + table.dbTable + ".Find(id);\n\t\t\t}");
                        w.WriteLine("\t\t\tcatch (Exception e){\n\t\t\t\treturn null;\n\t\t\t}\n\t\t}\n");
                        //delete
                        w.WriteLine("\t\tpublic void Delete(int id){");
                        w.WriteLine("\t\t\t"+table.dbTable+" entity = _context."+table.dbTable+".Find(id);");
                        w.WriteLine("\t\t\tif (entity != null){\n\t\t\t\t_context."+table.dbTable+".Remove(entity);\n\t\t\t\t_context.SaveChanges();\n\t\t\t}");
                        w.WriteLine("\t\t}");
                        //get data for children
                        foreach(var child in table.children)
                        {
                            w.WriteLine("\t\tpublic List<"+child.dbTable+"> Get"+child.dbTable+"Children(int id){");
                            w.WriteLine("\t\t\treturn _context."+table.dbTable+".Where(x=>x.Id==id).Include(x=>x."+child.dbTable+")." +
                                "SelectMany(x=>x."+child.dbTable+").ToList();\n\t\t}");
                        }
                        w.WriteLine("\t}\n}");
                    }
                }
            }
            foreach (NNModel nnModel in model.nnRelations)
            {
                using (FileStream fs = new FileStream(path + "\\" + nnModel.nnTable + "AccessLayer.cs", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("using System;\nusing " + project + ".Server.Models;\n" + "using " + project + ".Shared.Models;\nusing System.Collections.Generic;\n");
                        w.WriteLine("using System.Linq;\nusing Microsoft.EntityFrameworkCore;");
                        w.WriteLine("namespace " + project + ".Server.DataAccess\n{");
                        w.WriteLine("\tpublic class " + nnModel.nnTable + "AccessLayer\n{\n");
                        w.WriteLine("\t\tContext _context = new Context();\n");
                        //get for first
                        w.WriteLine("\t\tpublic List<" + nnModel.nnProps.table2 + "> Get"+nnModel.nnProps.table2+"(int id)\n\t\t{");
                        w.WriteLine("\t\t\ttry{\n\t\t\t\treturn _context."+nnModel.nnTable+".Where(x=>x."+nnModel.nnProps.attr1+" == id)." +
                            "Include(x=>x."+nnModel.nnProps.table2+ ").Select(x=>x." + nnModel.nnProps.table2 + ").Distinct().ToList();\n\t\t\t}");
                        w.WriteLine("\t\t\tcatch(Exception e){\n\t\t\t\treturn null;\n\t\t\t}\n\t\t}\n");
                        //get for second
                        w.WriteLine("\t\tpublic List<" + nnModel.nnProps.table1 + "> Get" + nnModel.nnProps.table1 + "(int id)\n\t\t{");
                        w.WriteLine("\t\t\ttry{\n\t\t\t\treturn _context." + nnModel.nnTable + ".Where(x=>x." + nnModel.nnProps.attr2 + " == id)." +
                            "Include(x=>x." + nnModel.nnProps.table1 + ").Select(x=>x." + nnModel.nnProps.table1 + ").Distinct().ToList();\n\t\t\t}");
                        w.WriteLine("\t\t\tcatch(Exception e){\n\t\t\t\treturn null;\n\t\t\t}\n\t\t}\n");
                        //get nn by ids
                        w.WriteLine("\t\tpublic " + nnModel.nnTable + " GetById(int id1,int id2){");
                        w.WriteLine("\t\t\ttry{\n\t\t\t\treturn _context." + nnModel.nnTable + "" +
                            ".Where(x=>x."+nnModel.nnProps.attr1+"==id1 && id2==x."+nnModel.nnProps.attr2+").SingleOrDefault();\n\t\t\t}");
                        w.WriteLine("\t\t\tcatch (Exception e){\n\t\t\t\treturn null;\n\t\t\t}\n\t\t}\n");
                        //delete nnTable
                        w.WriteLine("\t\tpublic void Delete(int id1, int id2){");
                        w.WriteLine("\t\t\t" + nnModel.nnTable + " entity = _context." + nnModel.nnTable + "" +
                            ".Where(x=>x."+nnModel.nnProps.attr1+" == id1 && id2 == x."+nnModel.nnProps.attr2+").SingleOrDefault();");
                        w.WriteLine("\t\t\tif (entity != null){\n\t\t\t\t_context." + nnModel.nnTable + ".Remove(entity);\n\t\t\t\t_context.SaveChanges();\n\t\t\t}");
                        w.WriteLine("\t\t}");
                        //post nnTable
                        w.WriteLine("\t\tpublic void Add(" + nnModel.nnTable + " model){");
                        w.WriteLine("\t\t\ttry{");
                        w.WriteLine("\t\t\t\t" + nnModel.nnTable + " entity = _context." + nnModel.nnTable + "" +
                            ".Where(x=>x." + nnModel.nnProps.attr1 + " == model."+nnModel.nnProps.attr1+" && model."+nnModel.nnProps.attr2+" == x." + nnModel.nnProps.attr2 + ").SingleOrDefault();");
                        w.WriteLine("\t\t\t\tif (entity == null){");
                        w.WriteLine("\t\t\t\t\t_context." + nnModel.nnTable + ".Add(model);");
                        w.WriteLine("\t\t\t\t\t_context.SaveChanges();\n\t\t\t\t}\n\t\t\t}");
                        w.WriteLine("\t\t\tcatch(Exception e){\n\n\t\t\t}\n\t\t}\n\n");
                        //update nnTable
                        w.WriteLine("\t\tpublic void Update(" + nnModel.nnTable + " " + nnModel.nnTable.ToLower() + "){");
                        w.WriteLine("\t\t\ttry{\n\t\t\t\tif(" + nnModel.nnTable.ToLower() + " != null){\n\t\t\t\t\t" +
                            "_context." + nnModel.nnTable + ".Update(" + nnModel.nnTable.ToLower() + ");");
                        w.WriteLine("\t\t\t\t\t_context.SaveChanges();\n\t\t\t\t}\n\t\t\t}");
                        w.WriteLine("\t\t\tcatch(Exception e){\n\n\t\t\t}\n\t\t}\n\n");

                        w.WriteLine("\t}\n}"); //closing for namespace and class
                    }
                }
            }
        }
    }
}
