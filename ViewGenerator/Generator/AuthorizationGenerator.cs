using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ViewGenerator.Models;

namespace ViewGenerator.Generator
{
    public static class AuthorizationGenerator
    {
        public static void generateAuthorization(TableModelCollection model, string path, string projectName)
        {
            Dictionary<string, List<int>> readPermissions = new Dictionary<string, List<int>>();
            Dictionary<string, List<int>> writePermissions = new Dictionary<string, List<int>>();

            foreach (TableModel table in model.tableModels)
            {
                readPermissions.Add(table.dbTable.ToLower(), new List<int>(table.readPermissions));
                writePermissions.Add(table.dbTable.ToLower(), new List<int>(table.writePermissions));
            }
            //authorization class
            using (FileStream fs = new FileStream(path + "\\AuthorizationStore.cs", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("using System.Collections.Generic;\n");
                    w.WriteLine("namespace " + projectName + ".Server\n{");
                    w.WriteLine("\tpublic static class AuthorizationStore\n\t{");
                    w.WriteLine("\t\tprivate static int roleId=0;");
                    w.WriteLine("\t\tprivate static Dictionary<string, List<int>> readPermissions = new Dictionary<string, List<int>>()\n\t\t{");
                    string s = "";
                    foreach (KeyValuePair<string, List<int>> pair in readPermissions)
                    {
                        s+="\t\t\t{ \"" + pair.Key + "\", new List<int>(){ " + string.Join(',', pair.Value) + " } },\n";
                    }
                    s = s.Substring(0, s.Length - 2);
                    w.WriteLine(s);
                    s = "";
                    w.WriteLine("\t\t};\n\t\tprivate static Dictionary<string, List<int>> writePermissions = new Dictionary<string, List<int>>()\n\t\t{");
                    foreach (KeyValuePair<string, List<int>> pair in writePermissions)
                    {
                        s+="\t\t\t{ \"" + pair.Key + "\", new List<int>(){ " + string.Join(',', pair.Value) + " } },\n";
                    }
                    s = s.Substring(0, s.Length - 2);
                    w.WriteLine(s);
                    w.WriteLine("\t\t};");
                    w.WriteLine("\t\tpublic static int getRoleId()\n\t\t{\n\t\t\treturn roleId;\n\t\t}");
                    w.WriteLine("\t\tpublic static void setRoleId(int id)\n\t\t{\n\t\t\troleId=id;\n\t\t}");
                    w.WriteLine("\t\tpublic static bool checkReadPermission(string controller)\n\t\t{");
                    w.WriteLine("\t\t\tif (readPermissions[controller].Contains(roleId))\n\t\t\t{\n\t\t\t\treturn true;\n\t\t\t}");
                    w.WriteLine("\t\t\treturn false;\n\t\t}");
                    w.WriteLine("\t\tpublic static bool checkWritePermissions(string controller)\n\t\t{");
                    w.WriteLine("\t\t\tif (writePermissions[controller].Contains(roleId))\n\t\t\t{\n\t\t\t\treturn true;\n\t\t\t}");
                    w.WriteLine("\t\t\treturn false;\n\t\t}");
                    w.WriteLine("\t}\n}");
                }
            }
            //accountcontroller
            using (FileStream fs = new FileStream(path + "\\AccountController.cs", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("using System.Linq;\nusing "+projectName+".Server.Models;");
                    w.WriteLine("using "+projectName+".Shared.Models;\nusing Microsoft.AspNetCore.Mvc;\n");
                    w.WriteLine("namespace "+projectName+".Server.Controllers\n{");
                    w.WriteLine("\tpublic class AccountController: Controller\n\t{");
                    w.WriteLine("\t\tContext _context = new Context();\n");
                    //login
                    w.WriteLine("\t\t[HttpPost]\n\t\t[Route(\"api/login\")]");
                    w.WriteLine("\t\tpublic IActionResult LogIn([FromBody] UserModel model)\n\t\t{");
                    w.WriteLine("\t\t\tvar user = _context.Person.Where(x=>model.password==x.Password && model.userName==x.UserName).SingleOrDefault();");
                    w.WriteLine("\t\t\tif(user != null)\n\t\t{");
                    w.WriteLine("\t\t\t\tAuthorizationStore.setRoleId(user.RoleId);\n\t\t\t\treturn new ObjectResult(user.RoleId);\n\t\t\t}");
                    w.WriteLine("\t\t\treturn new ObjectResult(0);\n\t\t}");
                    w.WriteLine("");
                    //register
                    w.WriteLine("\t\t[HttpPost]\n\t\t[Route(\"api/register\")]");
                    w.WriteLine("\t\tpublic IActionResult Register([FromBody] Person model)\n\t\t{");
                    w.WriteLine("\t\t\tif(ModelState.IsValid)\n\t\t\t{");
                    w.WriteLine("\t\t\t\t_context.Person.Add(model);\n\t\t\t\t_context.SaveChanges();");
                    w.WriteLine("\t\t\t\tAuthorizationStore.setRoleId(model.RoleId);");
                    w.WriteLine("\t\t\t\treturn new ObjectResult(model.RoleId);\n\t\t\t}");
                    w.WriteLine("\t\t\telse\n\t\t\t{\n\t\t\t\treturn new ObjectResult(0);\n\t\t\t}");
                    w.WriteLine("\n\t\t}");
                    w.WriteLine("\t}\n}");
                }
            }

            //log in screen
            using (FileStream fs = new FileStream(path + "\\Login.cshtml", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("@page \"/\"\n@page \"/login\"\n");
                    w.WriteLine("@inject HttpClient Http\n@inject Microsoft.AspNetCore.Blazor.Services.IUriHelper uriHelper\n");
                    w.WriteLine("<h5> @message </h5>\n\n<div> ");
                    w.WriteLine("\t<label> UserName </label>\n\t<input type=\"text\" bind=\"@UserName\" asp-for=\"username\" />");
                    w.WriteLine("\t<label> Password </label>\n\t<input type=\"password\" bind=\"@Password\" asp-for=\"password\" />");
                    w.WriteLine("\t<button onclick=\"LogIn\">Log in </button>\n</div>");
                    w.WriteLine("\n@functions{");
                    w.WriteLine("\tpublic string UserName {get; set;}\n\tpublic string Password{get; set;}\n\tpublic string message = \"\";");
                    w.WriteLine("\tpublic async Task LogIn()\n\t{");
                    w.WriteLine("\t\tvar data = new " + projectName + ".Shared.Models.UserModel {password = Password, userName= UserName};");
                    w.WriteLine("\t\tvar response = await Http.PostJsonAsync<int>(\"/api/login\", data);");
                    w.WriteLine("\t\tif (response.ToString()==\"0\")\n\t\t{");
                    w.WriteLine("\t\t\tmessage=\"User wasn't found, please check username and password\";\n\t\t}");
                    w.WriteLine("\t\telse\n\t\t{");
                    w.WriteLine("\t\t\tAuthorizationStore.setRoleId(response);\n\t\t\turiHelper.NavigateTo(\"/start\");\n\t\t}");
                    w.WriteLine("\t}\n}");
                }
            }
        }
    }
}
