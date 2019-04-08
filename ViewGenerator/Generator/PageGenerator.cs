using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ViewGenerator.Models;

namespace ViewGenerator.Generator
{
    public static class PageGenerator
    {
        public static void GenerateReadView(string viewsPath, TableModelCollection model, string projectName)
        {
            foreach (TableModel table in model.tableModels)
            {
                using (FileStream fs = new FileStream(viewsPath + "\\" + table.dbTable + ".cshtml", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("@using "+projectName+".Shared.Models");
                        w.WriteLine("@*\n\tput routes for page on top with @page /{wishedRoute}\n*@");
                        w.WriteLine();
                        foreach (var attr in table.atributes)
                        {
                            w.WriteLine("<p " + (attr.hidden ? "hidden" : "") + "><span> " + attr.name + "</span> @" + table.dbTable.ToLower() + "." + attr.name + "</p>");
                        }
                        w.WriteLine();
                        w.WriteLine("@functions{\n\t[Parameter]\n\t"+table.dbTable+" "+table.dbTable.ToLower()+"{get; set;}\n}");
                    }
                }
            }
        }

        public static void GenerateTableView(string viewsPath, TableModelCollection model, string projectName)
        {
            foreach (TableModel table in model.tableModels)
            {
                using (FileStream fs = new FileStream(viewsPath + "\\" + table.dbTable + "Table.cshtml", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("@using "+projectName+".Shared.Models //route to models");
                        w.WriteLine("@inject HttpClient Http ");
                        w.WriteLine();
                        w.WriteLine("@if (models==null) { \n\t <p><em>Loading...</em></p> \n}\n else {");
                        w.WriteLine("<table class=\"table\">");
                        w.WriteLine("\t<thead>");
                        w.WriteLine("\t\t<tr>");
                        foreach (var attr in table.atributes)
                        {
                            w.WriteLine("\t\t\t<td" + (attr.hidden ? " hidden" : "") + "> " + attr.name + "</td>");
                        }
                        w.WriteLine("\t\t</tr>");
                        w.WriteLine("\t</thead>");
                        w.WriteLine("\t<tbody>");
                        w.WriteLine("\t@foreach(var entity in @models){");
                        w.WriteLine("\t\t<tr>");
                        foreach (var attr in table.atributes)
                        {
                            w.WriteLine("\t\t\t<td> entity." + attr.name + "</td>");
                        }
                        w.WriteLine("\t\t\t<td><a href='/"+table.dbTable+"/edit/@entity.Id'>Edit</a> |<a href='/"+table.dbTable+"/delete/@entity.Id'>Delete</a></td>");
                        w.WriteLine("\t\t</tr>");
                        w.WriteLine("\t}");
                        w.WriteLine("\t</tbody>");
                        w.WriteLine("</table>");
                        w.WriteLine("}");
                        w.WriteLine("@functions{\n\n");
                        w.WriteLine("\tList<" + table.dbTable + "> models;");
                        w.WriteLine("\tprotected override async Task OnInitAsync()\n\t{ ");
                        w.WriteLine("\t\tmodels=await Http.GetJsonAsync<List<" + table.dbTable + ">>(  \" neki link \");");
                        w.WriteLine();
                        w.WriteLine();
                        w.WriteLine("\t}");
                        w.WriteLine("}");
                    }
                }
            }
        }
    }
}
