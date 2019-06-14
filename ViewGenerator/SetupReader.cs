using Microsoft.DotNet.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ViewGenerator.Models;

namespace ViewGenerator
{
    static class SetupReader
    {

        public static TableModelCollection ReadData(string path)
        {
            JObject jObject = new JObject();
            using (StreamReader s = File.OpenText(path))
            {
                using (JsonTextReader reader = new JsonTextReader(s))
                {
                    jObject = (JObject)JToken.ReadFrom(reader);

                }
            }
            var tableCollection = new TableModelCollection();
            foreach (var table in jObject)
            {
                if (table.Key == "validation")
                {
                    tableCollection.validation = table.Value.ToObject<bool>();
                }
                else if (table.Key == "n-n")
                {
                    foreach(var obj in JObject.Parse(table.Value.ToString()))
                    {
                        var nnModel = new NNModel();
                        nnModel.nnTable = obj.Key;
                        foreach(var prop in JObject.Parse(obj.Value.ToString()))
                        {
                            if (prop.Key == "props")
                            {
                                nnModel.nnProps = prop.Value.ToObject<NNProps>();
                            }
                            else if (prop.Key == "attr")
                            {
                                foreach (var att in JObject.Parse(prop.Value.ToString()))
                                {
                                    nnModel.atributes.Add(att.Value.ToObject<AtributeModel>());
                                }
                            }
                        }
                        tableCollection.nnRelations.Add(nnModel);
                    }
                }
                else
                {
                    var tableModel = new TableModel();
                    tableModel.dbTable = table.Key;

                    foreach (var attr in JObject.Parse(table.Value.ToString()))
                    {
                        //so generator could generate authorization
                        if (attr.Key == "read")
                        {
                            tableModel.readPermissions = attr.Value.ToObject<int[]>();
                        }
                        else if (attr.Key == "write")
                        {
                            tableModel.writePermissions = attr.Value.ToObject<int[]>();
                        }
                        //get children
                        else if (attr.Key == "children")
                        {
                            foreach (var childTable in JObject.Parse(attr.Value.ToString()))
                            {
                                var child = new ChildModel();
                                child.dbTable = childTable.Key;
                                foreach (var childAttr in JObject.Parse(childTable.Value.ToString()))
                                {
                                    child.atributes.Add(childAttr.Value.ToObject<AtributeModel>());
                                }
                                tableModel.children.Add(child);
                            }
                        } //take n-n relations
                        else if (attr.Key == "n-n")
                        {
                            foreach (var nn in JObject.Parse(attr.Value.ToString()))
                            {
                                var nnModel = new NNRelationModel();
                                nnModel.nnTable = nn.Key;
                                foreach (var nnDataConn in JObject.Parse(nn.Value.ToString()))
                                {

                                    nnModel.atributes.Add(nnDataConn.Value.ToObject<AtributeModel>());
                                }
                                tableModel.nNRelations.Add(nnModel);
                            }
                        }  //take attributes
                        else if (attr.Key != "children")
                        {
                            tableModel.atributes.Add(attr.Value.ToObject<AtributeModel>());
                        }
                    }
                    tableCollection.tableModels.Add(tableModel);
                }
            }
            return tableCollection;
        }
    }
}
