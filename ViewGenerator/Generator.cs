using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ViewGenerator.Models;

namespace ViewGenerator
{
    static class Generator
    {
        public static void GenerateView(string viewsPath, TableModelCollection model)
        {
            foreach (TableModel table in model.tableModels)
            {
                using (FileStream fs = new FileStream(viewsPath + "\\"+table.dbTable+".cshtml", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("@*\n\tput routes for page on top with @page /{wishedRoute}\n*@");
                        w.WriteLine();
                        foreach(var attr in table.atributes)
                        {
                            w.WriteLine("<p "+(attr.hidden?"hidden":"")+"><span> "+attr.name +"</span> @"+table.dbTable+"."+attr.name+"</p>");
                        }
                        w.WriteLine();
                        w.WriteLine("@functions{\n\n}");
                    }
                }
            }
        }
    }
}
