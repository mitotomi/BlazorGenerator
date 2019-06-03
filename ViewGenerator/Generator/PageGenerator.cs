using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                        w.WriteLine("@page \"/" + table.dbTable.ToLower() + "s/{id}\"");
                        w.WriteLine("@inject HttpClient Http\n@inject Microsoft.AspNetCore.Blazor.Services.IUriHelper uriHelper");
                        w.WriteLine("@*\n\tput routes for page on top with @page /{wishedRoute}\n*@");
                        w.WriteLine();
                        foreach (var attr in table.atributes)
                        {
                            w.WriteLine("<p " + (attr.hidden ? "hidden" : "") + "><span> " + attr.name + "</span> @model." + attr.name + "</p>");
                        }
                        foreach (var child in table.children)
                        {
                            w.WriteLine("\n<h3>" + child.dbTable + "s</h3>\n");
                            w.WriteLine("<table>\n\t<thead>\n\t\t<tr>");
                            foreach (var attr in child.atributes)
                            {
                                w.WriteLine("\t\t\t<td" + (attr.hidden ? " hidden" : "") + "> " + attr.name + "</td>");
                            }
                            w.WriteLine("\t\t</tr>\n\t</thead>");
                            w.WriteLine("\t<tbody>");
                            w.WriteLine("\t@foreach(var entity in @model." + child.dbTable + "){");
                            w.WriteLine("\t\t<tr>");
                            foreach (var attr in child.atributes)
                            {
                                if (table.atributes.IndexOf(attr) != 1)
                                {
                                    w.WriteLine("\t\t\t<td " + (attr.hidden ? " hidden" : "") + "> @entity." + attr.name + "</td>");
                                }
                                else
                                {
                                    w.WriteLine("\t\t\t<td " + (attr.hidden ? " hidden" : "") + "><a href=\"/" + child.dbTable.ToLower() +
                                        "s/@entity.Id\">" + " @entity." + attr.name + "</a></td>");
                                }
                            }
                            w.WriteLine("\t\t\t<td><button onclick=\"@(e=>Edit(entity.Id, \"" + child.dbTable + "\"))\">Edit</button> |<button onclick=\"@(e=>Delete(entity.Id, \"" + child.dbTable + "\"))\">Delete</button></td>");
                            w.WriteLine("\t\t</tr>");
                            w.WriteLine("\t}");
                            w.WriteLine("\t</tbody>\n</table>");
                        }
                        Dictionary<string, string> nnDict = new Dictionary<string, string>();

                        foreach (var nnRelation in table.nNRelations)
                        {
                            nnDict.Add(nnRelation.nnTable, model.nnRelations.Where(x => x.nnTable == nnRelation.nnTable).SingleOrDefault().nnProps.table1);

                            w.WriteLine("\n<h5>" + nnRelation.nnTable + "s</h5>\n");
                            if (nnDict[nnRelation.nnTable] == table.dbTable)
                            {
                                w.WriteLine("<button onClick=@( () => Createnn(\"" + nnRelation.nnTable.ToLower() + "\"))>Create</button>");
                            }
                            else
                            {
                                w.WriteLine("<button onClick=@( () => nnCreate(\"" + nnRelation.nnTable.ToLower() + "\"))>Create</button>");
                            }
                            w.WriteLine("<table>\n\t<thead>\n\t\t<tr>");
                            foreach (var attr in nnRelation.atributes)
                            {
                                w.WriteLine("\t\t\t<td" + (attr.hidden ? " hidden" : "") + "> " + attr.name + "</td>");
                            }
                            w.WriteLine("\t\t</tr>\n\t</thead>");
                            w.WriteLine("\t<tbody>");
                            w.WriteLine("\t@foreach(var entity in " + nnRelation.nnTable.ToLower() + "s){");
                            w.WriteLine("\t\t<tr>");
                            foreach (var attr in nnRelation.atributes)
                            {
                                if (table.atributes.IndexOf(attr) != 1)
                                {
                                    w.WriteLine("\t\t\t<td " + (attr.hidden ? " hidden" : "") + "> @entity." + attr.name + "</td>");
                                }
                                else
                                {
                                    w.WriteLine("\t\t\t<td " + (attr.hidden ? " hidden" : "") + "><a href=\"/" + nnRelation.nnTable.ToLower() +
                                        "s/@entity.Id\">" + " @entity." + attr.name + "</a></td>");
                                }
                            }
                            if (nnDict[nnRelation.nnTable] == table.dbTable)
                            {
                                w.WriteLine("\t\t\t<td><button onclick=\"@(e=>nnEdit(entity.Id, \"" + nnRelation.nnTable + "\"" +
                                    "))\">Edit</button> |<button onclick=\"@(e=>nnDelete(entity.Id, \"" + nnRelation.nnTable + "\"))\">Delete</button></td>");

                            }
                            else
                            {
                                w.WriteLine("\t\t\t<td><button onclick=\"@(e=>Editnn(entity.Id, \"" + nnRelation.nnTable + "\" " +
                                    "))\">Edit</button> |<button onclick=\"@(e=>Deletenn(entity.Id, \"" + nnRelation.nnTable + "\" ))\">Delete</button></td>");
                            }
                            w.WriteLine("\t\t</tr>");
                            w.WriteLine("\t}");
                            w.WriteLine("\t</tbody>\n</table>");
                        }
                        w.WriteLine();
                        w.WriteLine("@functions{\n\t[Parameter]\n\tprivate string Id {get; set;}\n\n\t" + projectName + ".Shared.Models." + table.dbTable + " " +
                            "model = new " + projectName + ".Shared.Models." + table.dbTable + "();");
                        foreach (var nnRelation in table.nNRelations)
                        {
                            string thisTable = table.dbTable;
                            string nnRelationTable = nnRelation.nnTable;
                            var relationModel = model.nnRelations.Where(x => x.nnTable == nnRelationTable).SingleOrDefault();
                            var otherTable = relationModel.nnProps.table1 == thisTable ? relationModel.nnProps.table2 : relationModel.nnProps.table1;
                            w.WriteLine("ICollection<" + projectName + ".Shared.Models." + otherTable + "> " + relationModel.nnTable.ToLower() + "s = new List<" + projectName + ".Shared.Models." + otherTable + ">();");
                        }

                        w.WriteLine("\tprotected override async Task OnInitAsync(){\n\t\tmodel=await Http.GetJsonAsync<" + projectName + ".Shared.Models." + table.dbTable + ">(\"/api/" + table.dbTable.ToLower() + "/\"+Id);");
                        foreach (var child in table.children)
                        {
                            w.WriteLine("\t\tmodel." + child.dbTable + " = await Http.GetJsonAsync<List<" + projectName + ".Shared.Models." + child.dbTable + ">>(\"/api/" + table.dbTable.ToLower() + "/" + child.dbTable.ToLower() + "/\"+Id);");
                        }

                        foreach (var nnRelation in table.nNRelations)
                        {

                            string thisTable = table.dbTable;
                            string nnRelationTable = nnRelation.nnTable;
                            var relationModel = model.nnRelations.Where(x => x.nnTable == nnRelationTable).SingleOrDefault();
                            var otherTable = relationModel.nnProps.table1 == thisTable ? relationModel.nnProps.table2 : relationModel.nnProps.table1;
                            w.WriteLine("\t\t" + relationModel.nnTable.ToLower() + "s = await Http.GetJsonAsync<List<" + projectName + ".Shared.Models." + otherTable + ">>(\"/api/" + nnRelationTable.ToLower() + "/" + otherTable.ToLower() + "/\"+Id);");
                        }
                        w.WriteLine("\t}");
                        if (table.children.Count > 0)
                        {
                            w.WriteLine("\tvoid Edit(int id, string table){\n\t\turiHelper.NavigateTo(\"/\"+table.ToLower()+\"/\"+id);\n\t}");
                            w.WriteLine("\tvoid Delete(int id, string table){\n\t\turiHelper.NavigateTo(\"/\" + table.ToLower() + \"/delete/\"+id);\n\t}");
                        }
                        if (table.nNRelations.Count > 0)
                        {

                            w.WriteLine("\tvoid Editnn(int id, string table){\n\t\turiHelper.NavigateTo(\"/\"+table.ToLower()+\"/"+table.dbTable.ToLower()+"/\"+id+\"/\"+Id);\n\t}");
                            w.WriteLine("\tvoid Deletenn(int id, string table){\n\t\turiHelper.NavigateTo(\"/\" + table.ToLower() + \"/delete/\"+id+\"/\"+Id);\n\t}");

                            w.WriteLine("\tvoid nnEdit(int id, string table){\n\t\turiHelper.NavigateTo(\"/\"+table.ToLower()+\"/" + table.dbTable.ToLower() + "/\"+Id+\"/\"+id);\n\t}");
                            w.WriteLine("\tvoid nnDelete(int id, string table){\n\t\turiHelper.NavigateTo(\"/\" + table.ToLower() + \"/delete/\"+Id+\"/\"+id);\n\t}");

                            w.WriteLine("\tvoid Createnn(string nnTable){\n\t\turiHelper.NavigateTo(\"/\"+nnTable+\"/" + table.dbTable.ToLower() + "/\"+Id+\"/0\");\n\t}");
                            w.WriteLine("\tvoid nnCreate(string nnTable){\n\t\turiHelper.NavigateTo(\"/\"+nnTable+\"/" + table.dbTable.ToLower() + "/0/\"+Id);\n\t}");

                        }
                        w.WriteLine("}");
                    }
                }
            }
        }

        public static void GenerateDelete(string viewsPath, TableModelCollection model, string projectName)
        {
            foreach (TableModel table in model.tableModels)
            {
                using (FileStream fs = new FileStream(viewsPath + "\\" + table.dbTable + "Delete.cshtml", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("@page \"/" + table.dbTable.ToLower() + "/delete/{id}\"");
                        w.WriteLine("@inject HttpClient Http\n@inject Microsoft.AspNetCore.Blazor.Services.IUriHelper uriHelper");
                        w.WriteLine("<h1>Delete " + table.dbTable + "</h1>\n");
                        w.WriteLine("Are you sure you want to delete this entity");
                        w.WriteLine("\t\t\t<td><button onclick=\"@Yes\">Yes</button> |<button onclick=\"@No\">No</button></td>");
                        w.WriteLine("@functions{");
                        w.WriteLine("\t[Parameter]\n\tprivate string Id {get; set;}");
                        w.WriteLine("\tpublic async Task Yes(){\n\t\tawait Http.DeleteAsync(\"/api/" + table.dbTable.ToLower() + "/delete/\"+Id);" +
                            "\n\t\turiHelper.NavigateTo(\"/" + table.dbTable.ToLower() + "s\");\n\t}");
                        w.WriteLine("\tpublic void No(){\n\t\turiHelper.NavigateTo(\"/" + table.dbTable.ToLower() + "s\");\n\t}");
                        w.WriteLine("}");
                    }
                }
            }

            foreach (NNModel nNModel in model.nnRelations)
            {
                using (FileStream fs = new FileStream(viewsPath + "\\" + nNModel.nnTable + "Delete.cshtml", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("@page \"/" + nNModel.nnTable.ToLower() + "/delete/{id}\"");
                        w.WriteLine("@inject HttpClient Http\n@inject Microsoft.AspNetCore.Blazor.Services.IUriHelper uriHelper");
                        w.WriteLine("<h1>Delete " + nNModel.nnTable + "</h1>\n");
                        w.WriteLine("Are you sure you want to delete this entity");
                        w.WriteLine("\t\t\t<td><button onclick=\"@Yes\">Yes</button> |<button onclick=\"@No\">No</button></td>");
                        w.WriteLine("@functions{");
                        w.WriteLine("\t[Parameter]\n\tprivate string Id {get; set;}");
                        w.WriteLine("\tpublic async Task Yes(){\n\t\tawait Http.DeleteAsync(\"/api/" + nNModel.nnTable.ToLower() + "/delete/\"+Id);" +
                            "\n\t\turiHelper.NavigateTo(\"/" + nNModel.nnTable.ToLower() + "s\");\n\t}");
                        w.WriteLine("\tpublic void No(){\n\t\turiHelper.NavigateTo(\"/" + nNModel.nnTable.ToLower() + "s\");\n\t}");
                        w.WriteLine("}");
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
                        w.WriteLine("@inject HttpClient Http\n@inject Microsoft.AspNetCore.Blazor.Services.IUriHelper uriHelper");
                        w.WriteLine("<h1>Edit " + table.dbTable + "</h1>\n");
                        if (table.atributes.Where(x => x.foreignKey == true).Count() > 0)
                        {
                            w.WriteLine("<h6>@message</h6>");
                        }
                        w.WriteLine("<form onsubmit=\"@Post\">\n<table>\n\t<tbody>");
                        foreach (var attr in table.atributes)
                        {
                            if (!attr.foreignKey)
                            {
                                w.WriteLine("\t\t<tr>\n\t\t\t<td>");
                                if (!attr.hidden)
                                {
                                    w.WriteLine("\t\t\t\t<label>" + attr.name + "</label>");
                                }
                                w.WriteLine("\t\t\t\t<input type=\"" + attr.type + "\" bind=\"@model." + attr.name + "\"" +
                                    (attr.type == "date" ? " format-value=\"yyyy-MM-dd\"" : "") + " asp-for=\"" + attr.name + "\" " +
                                    (attr.hidden ? "hidden" : "") + "/>");
                                w.WriteLine("\t\t\t</td>\n\t\t</tr>");
                            }
                            else
                            {
                                w.WriteLine("\t\t<tr>\n\t\t\t<td>");
                                w.WriteLine("\t\t\t\t<label>" + attr.name + "</label>");
                                w.WriteLine("\t\t\t\t<select bind=\"@model." + attr.name + "\">\n\t\t\t\t\t<option value=\"\">Choose value</option>");
                                w.WriteLine("\t\t\t\t\t@foreach(var option in options" + attr.name.ToLower() + "){\n\t\t\t\t\t\t<option value=\"@option.Key\">@option.Value</option>");
                                w.WriteLine("\t\t\t\t\t}\n\t\t\t\t</select>\n\t\t\t</td>\n\t\t</tr>");
                            }
                        }
                        w.WriteLine("\t</tbody>\n</table>");
                        w.WriteLine("\t<button type=\"submit\" class=\"btn btn - success\">Save</button>\n</form>\n\n@functions{");
                        w.WriteLine("\t[Parameter]\n\tprivate string Id {get; set;}\n\n\t" + projectName + ".Shared.Models." + table.dbTable + " " +
                            "model = new " + projectName + ".Shared.Models." + table.dbTable + "();");
                        foreach (var attr in table.atributes.Where(x => x.foreignKey == true))
                        {
                            w.WriteLine("\tList<" + projectName + ".Shared.Models.SelectListItem> options" + attr.name.ToLower() + " = new List<" + projectName + ".Shared.Models.SelectListItem>();");
                        }
                        w.WriteLine("\tstring message = \"\";");
                        w.WriteLine("\tprotected override async Task OnInitAsync(){\n\t\tmodel=await Http.GetJsonAsync<" + projectName + ".Shared.Models." + table.dbTable + ">(\"/api/" + table.dbTable.ToLower() + "/\"+Id);");
                        foreach (var attr in table.atributes.Where(x => x.foreignKey == true))
                        {
                            w.WriteLine("\t\toptions" + attr.name.ToLower() + " = await Http.GetJsonAsync<List<" + projectName + ".Shared.Models.SelectListItem>>(\"/api/" + table.dbTable.ToLower() + "s/" + attr.fkTable.ToLower() + attr.fkValue.ToLower() + "\");");
                        }
                        w.WriteLine("\t}");
                        if (table.atributes.Any(x => x.foreignKey == true))
                        {
                            string condition = "";
                            foreach (var attr in table.atributes.Where(x => x.foreignKey == true))
                            {
                                condition += "model." + attr.name + " == 0 ||";
                            }
                            condition = condition.Substring(0, condition.Length - 2);
                            w.WriteLine("\tpublic async Task Post(){\n\t\ttry{\n\t\t\tif(" + condition + "){\n\t\t\t\tmessage=\"Please, fill all fields\";\n\t\t\t}");
                            w.WriteLine("\t\t\telse if(model.Id==0){\n\t\t\t\tawait Http.SendJsonAsync(HttpMethod.Post, \"/api/" + table.dbTable.ToLower() + "/create\", model);" +
                                "\n\t\t\t\turiHelper.NavigateTo(\"/" + table.dbTable.ToLower() + "s\");\n\t\t\t}");
                        }
                        else
                        {
                            w.WriteLine("\tpublic async Task Post(){\n\t\ttry{\n\t\t\tif(model.Id==0){\n\t\t\t\tawait Http.SendJsonAsync(HttpMethod.Post, \"/api/" + table.dbTable.ToLower() + "/create\",model);" +
                                "\n\t\t\t\t\turiHelper.NavigateTo(\"/" + table.dbTable.ToLower() + "s\");\n\t\t\t}");
                        }
                        w.WriteLine("\t\t\telse{\n\t\t\t\tawait Http.SendJsonAsync(HttpMethod.Post, \"/api/" + table.dbTable.ToLower() + "/edit\",model);" +
                            "\n\t\t\turiHelper.NavigateTo(\"/" + table.dbTable.ToLower() + "s\");\n\t\t\t}");
                        w.WriteLine("\t\t}\n\t\tcatch(Exception e){\n\t\t\tConsole.WriteLine(e.Message);\n\t\t\tthrow;\n\t\t}\n\t}\n}");
                    }
                }
            }

            foreach (NNModel nNModel in model.nnRelations)
            {
                using (FileStream fs = new FileStream(viewsPath + "\\" + nNModel.nnTable + "Create.cshtml", FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine("@page \"/" + nNModel.nnTable.ToLower() + "/{table}/{id1}/{id2}\"");
                        w.WriteLine("@page \"/"+nNModel.nnTable.ToLower()+"/{id1}/{id2}");
                        w.WriteLine("@inject HttpClient Http\n@inject Microsoft.AspNetCore.Blazor.Services.IUriHelper uriHelper");
                        w.WriteLine("<h1>Edit " + nNModel.nnTable + "</h1>\n");
                        w.WriteLine("<h6>@message</h6>");
                        w.WriteLine("<form onsubmit=\"@Post\">\n<table>\n\t<tbody>");
                        foreach (var attr in nNModel.atributes)
                        {
                            if (!attr.foreignKey)
                            {
                                w.WriteLine("\t\t<tr>\n\t\t\t<td>");
                                if (!attr.hidden)
                                {
                                    w.WriteLine("\t\t\t\t<label>" + attr.name + "</label>");
                                }
                                w.WriteLine("\t\t\t\t<input type=\"" + attr.type + "\" bind=\"@model." + attr.name + "\"" +
                                    (attr.type == "date" ? " format-value=\"yyyy-MM-dd\"" : "") + " asp-for=\"" + attr.name + "\" " +
                                    (attr.hidden ? "hidden" : "") + "/>");
                                w.WriteLine("\t\t\t</td>\n\t\t</tr>");
                            }
                            else
                            {
                                w.WriteLine("\t\t<tr>\n\t\t\t<td>");
                                w.WriteLine("\t\t\t\t<label>" + attr.name + "</label>");
                                w.WriteLine("\t\t\t\t<select bind=\"@model." + attr.name + "\">\n\t\t\t\t\t<option value=\"\">Choose value</option>");
                                w.WriteLine("\t\t\t\t\t@foreach(var option in options" + attr.name.ToLower() + "){\n\t\t\t\t\t\t<option value=\"@option.Key\">@option.Value</option>");
                                w.WriteLine("\t\t\t\t\t}\n\t\t\t\t</select>\n\t\t\t</td>\n\t\t</tr>");
                            }
                        }
                        w.WriteLine("\t</tbody>\n</table>");
                        w.WriteLine("\t<button type=\"submit\" class=\"btn btn - success\">Save</button>\n</form>\n\n@functions{");
                        w.WriteLine("\t[Parameter]\n\tprivate string Id1 {get; set;} \n\t[Parameter]\n\tprivate string Id2 {get; set;}" +
                            "\n\t[Parameter]\n\tprivate string Table{get; set;}" +
                            "\n\n\t" + projectName + ".Shared.Models." + nNModel.nnTable + " " +
                            "model = new " + projectName + ".Shared.Models." + nNModel.nnTable + "();");
                        foreach (var attr in nNModel.atributes.Where(x => x.foreignKey == true))
                        {
                            w.WriteLine("\tList<" + projectName + ".Shared.Models.SelectListItem> options" + attr.name.ToLower() + " = new List<" + projectName + ".Shared.Models.SelectListItem>();");
                        }
                        w.WriteLine("\tstring message = \"\";");
                        w.WriteLine("\tprotected override async Task OnInitAsync(){\n\t\tmodel=await Http.GetJsonAsync<" + projectName + ".Shared.Models." + nNModel.nnTable + ">(\"/api/" + nNModel.nnTable.ToLower() + "/\"+Id1+\"/\"+Id2);");
                        foreach (var attr in nNModel.atributes.Where(x => x.foreignKey == true))
                        {
                            w.WriteLine("\t\toptions" + attr.name.ToLower() + " = await Http.GetJsonAsync<List<" + projectName + ".Shared.Models.SelectListItem>>(\"/api/" + nNModel.nnTable.ToLower() + "s/" + attr.fkTable.ToLower() + attr.fkValue.ToLower() + "\");");
                        }
                        w.WriteLine("\t}");
                        if (nNModel.atributes.Any(x => x.foreignKey == true))
                        {
                            string condition = "";
                            foreach (var attr in nNModel.atributes.Where(x => x.foreignKey == true))
                            {
                                condition += "model." + attr.name + " == 0 ||";
                            }
                            condition = condition.Substring(0, condition.Length - 2);
                            w.WriteLine("\tpublic async Task Post(){\n\t\ttry{\n\t\t\tif(" + condition + "){\n\t\t\t\tmessage=\"Please, fill all fields\";\n\t\t\t}");
                            w.WriteLine("\t\t\telse if(model.Id==0){\n\t\t\t\tawait Http.SendJsonAsync(HttpMethod.Post, \"/api/" + nNModel.nnTable.ToLower() + "/create\", model);" +
                                "\n\t\t\t\turiHelper.NavigateTo(\"/\"+Table+\"s\");\n\t\t\t}");
                        }
                        else
                        {
                            w.WriteLine("\tpublic async Task Post(){\n\t\ttry{\n\t\t\tif(model.Id==0){\n\t\t\t\tawait Http.SendJsonAsync(HttpMethod.Post, \"/api/" + nNModel.nnTable.ToLower() + "/create\",model);" +
                                "\n\t\t\t\t\turiHelper.NavigateTo(\"/\"+Table+\"s\");\n\t\t\t}");
                        }
                        w.WriteLine("\t\t\telse{\n\t\t\t\tawait Http.SendJsonAsync(HttpMethod.Post, \"/api/" + nNModel.nnTable.ToLower() + "/edit\",model);" +
                            "\n\t\t\turiHelper.NavigateTo(\"/\"+Table+\"s\");\n\t\t\t}");
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
                        w.WriteLine("@inject HttpClient Http\n@inject Microsoft.AspNetCore.Blazor.Services.IUriHelper uriHelper");
                        w.WriteLine();
                        w.WriteLine("@if (models==null) { \n\t <p><em>Loading...</em></p> \n}\n else {");
                        w.WriteLine("\t<button onclick=\"@Create\">Create</button>");
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
                            if (table.atributes.IndexOf(attr) != 1)
                            {
                                w.WriteLine("\t\t\t<td " + (attr.hidden ? " hidden" : "") + "> @entity." + attr.name + "</td>");
                            }
                            else
                            {
                                w.WriteLine("\t\t\t<td " + (attr.hidden ? " hidden" : "") + "><a href=\"/" + table.dbTable.ToLower() +
                                    "s/@entity.Id\">" + " @entity." + attr.name + "</a></td>");
                            }
                        }
                        w.WriteLine("\t\t\t<td><button onclick=\"@(e=>Edit(entity.Id))\">Edit</button> |<button onclick=\"@(e=>Delete(entity.Id))\">Delete</button></td>");
                        w.WriteLine("\t\t</tr>");
                        w.WriteLine("\t}");
                        w.WriteLine("\t</tbody>");
                        w.WriteLine("</table>");
                        w.WriteLine("}");
                        w.WriteLine("@functions{\n\n");
                        w.WriteLine("\tList<" + projectName + ".Shared.Models." + table.dbTable + "> models;");
                        w.WriteLine("\tprotected override async Task OnInitAsync()\n\t{ ");
                        w.WriteLine("\t\tmodels=await Http.GetJsonAsync<List<" + projectName + ".Shared.Models." + table.dbTable + ">>(\"/api/" + table.dbTable + "s\");");
                        w.WriteLine("\t}");
                        w.WriteLine("\tvoid Create(){\n\t\turiHelper.NavigateTo(\"/" + table.dbTable.ToLower() + "/0\");\n\t}");
                        w.WriteLine("\tvoid Edit(int id){\n\t\turiHelper.NavigateTo(\"/" + table.dbTable.ToLower() + "/\"+id);\n\t}");
                        w.WriteLine("\tvoid Delete(int id){\n\t\turiHelper.NavigateTo(\"/" + table.dbTable.ToLower() + "/delete/\"+id);\n\t}");
                        w.WriteLine();
                        w.WriteLine("}");
                    }
                }
            }
        }
    }
}
