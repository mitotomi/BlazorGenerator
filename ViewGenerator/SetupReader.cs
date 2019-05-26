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
                var tableModel = new TableModel();
                tableModel.dbTable = table.Key;
                foreach (var attr in JObject.Parse(table.Value.ToString()))
                {
                    //get children
                    if (attr.Key == "children")
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
                            nnModel.relationTable = nn.Key;
                            foreach (var keys in JObject.Parse(nn.Value.ToString()))
                            {
                                if (keys.Key == "mainAttribute")
                                {
                                    nnModel.mainAttribute = keys.Value.ToString();
                                }
                                else if (keys.Key == "endAttribute")
                                {
                                    nnModel.endAttribute = keys.Value.ToString();
                                }
                                else
                                {
                                    nnModel.endTable.dbTable = keys.Key;
                                    foreach (var nnAttr in JObject.Parse(keys.Value.ToString()))
                                    {
                                        nnModel.endTable.atributes.Add(nnAttr.Value.ToObject<AtributeModel>());
                                    }
                                }
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
            return tableCollection;
        }
    }
}
