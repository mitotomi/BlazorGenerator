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
        }
    }
}
