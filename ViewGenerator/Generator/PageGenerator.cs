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
                        w.WriteLine("@using " + projectName + ".Shared.Models");
                        w.WriteLine("@*\n\tput routes for page on top with @page /{wishedRoute}\n*@");
                        w.WriteLine();
                        foreach (var attr in table.atributes)
                        {
                            w.WriteLine("<p " + (attr.hidden ? "hidden" : "") + "><span> " + attr.name + "</span> @" + table.dbTable.ToLower() + "." + attr.name + "</p>");
                        }
                        w.WriteLine();
                        w.WriteLine("@functions{\n\t[Parameter]\n\t" + projectName + ".Shared.Models." + table.dbTable + " " + table.dbTable.ToLower() + "{get; set;}\n}");
                    }
                }
            }
        }

        public static void GenerateCreateUpdate(string viewsPath, TableModelCollection model, string projectName)
        {
            foreach (TableModel table in model.tableModels)
            {
                using (FileStream fs = new FileStream(viewsPath + "\\" + table.dbTable + "Create.cshtml", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("@page \"/" + table.dbTable.ToLower() + "/{id}\"");
                        w.WriteLine("@inject HttpClient Http");
                        w.WriteLine("<h1>Edit " + table.dbTable + "</h1>\n");
                        w.WriteLine("<table>\n\t<tbody>");
                        w.WriteLine("\t<button onclick=\"@create\">Create</button>");
                        foreach (var attr in table.atributes)
                        {
                            w.WriteLine("\t\t<td>\n\t\t\t<tr>");
                            if (!attr.hidden)
                            {
                                w.WriteLine("\t\t\t\t<label>" + attr.name + "</label>");
                            }
                            w.WriteLine("\t\t\t\t<input type=\"" + attr.type + "\" bind=\"@model." + attr.name + "\" asp-for=\"" + attr.name + "\" " + (attr.hidden ? "hidden" : "") + "/>");
                            w.WriteLine("\t\t\t</td>\n\t\t</tr>");
                        }
                        w.WriteLine("\t\t<td>\n\t\t\t<tr>");
                        w.WriteLine("\t<input type=\"submit\" class=\"btn btn - success\" value=\"Save\" onClick=\"@Post\"/>");
                        w.WriteLine("\t\t\t</td>\n\t\t</tr>");
                        w.WriteLine("\t</tbody>\n</table>\n\n@functions{");
                        w.WriteLine("\t[Parameter]\n\tprivate string Id {get; set;}\n\n\t" + projectName + ".Shared.Models." + table.dbTable + " " +
                            "model = new " + projectName + ".Shared.Models." + table.dbTable + "()");
                        w.WriteLine("\tprotected override async Task OnInitAsync(){\n\t\tmodel=await Http.GetJsonAsync<>(\"/api/" + table.dbTable.ToLower() + "s/\"+Id);\n\t}");
                        w.WriteLine("\tpublic async Task Post(){\n\t\ttry{\n\t\t\tif(model.Id==0){\n\t\t\t\tawait Http.SendJsonAsync(HttpMethod.Post, \"/api/" + table.dbTable.ToLower() + "/create\",model);");
                        w.WriteLine("\t\t\t}\n\t\t\telse{\n\t\t\t\tawait Http.SendJsonAsync(HttpMethod.Post, \"/api/" + table.dbTable.ToLower() + "/edit\",model);\n\t\t\t}");
                        w.WriteLine("\t\t}\n\t\tcatch(Exception e){\n\t\t\tConsole.WriteLine(e.Message);\n\t\t\tthrow;\n\t\t}\n\t}\n}");
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
                        w.WriteLine("@page \"/" + table.dbTable.ToLower() + "s\"");
                        w.WriteLine("@using " + projectName + ".Shared.Models");
                        w.WriteLine("@inject HttpClient Http\n@inject IUriHelper uriHelper ");
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
                            w.WriteLine("\t\t\t<td> @entity." + attr.name + "</td>");
                        }
                        w.WriteLine("\t\t\t<td><a href='/" + table.dbTable + "/edit/@entity.Id'>Edit</a> |<a href='/" + table.dbTable + "/delete/@entity.Id'>Delete</a></td>");
                        w.WriteLine("\t\t</tr>");
                        w.WriteLine("\t}");
                        w.WriteLine("\t</tbody>");
                        w.WriteLine("</table>");
                        w.WriteLine("}");
                        w.WriteLine("@functions{\n\n");
                        w.WriteLine("\tList<" + projectName + ".Shared.Models." + table.dbTable + "> models;");
                        w.WriteLine("\tprotected override async Task OnInitAsync()\n\t{ ");
                        w.WriteLine("\t\tmodels=await Http.GetJsonAsync<List<" + table.dbTable + ">>(\"/api/" + table.dbTable + "s\");");
                        w.WriteLine("\t}");
                        w.WriteLine("\tvoid Create(){\n\t\turiHelper.NavigateTo(\"/" + table.dbTable.ToLower() + "/0\")");
                        w.WriteLine();
                        w.WriteLine("}");
                    }
                }
            }
        }
    }
}
