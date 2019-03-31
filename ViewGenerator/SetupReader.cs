using Microsoft.DotNet.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ViewGenerator
{
    class SetupReader
    {

        public void readData()
        {
            JObject jObject = new JObject();
            using (StreamReader s = File.OpenText(@"C:\Users\Tomislav\Documents\Visual Studio 2017\Projects\Master_v2\Master_v2.Server\generatorConfig.json"))
            {
                using (JsonTextReader reader = new JsonTextReader(s))
                {
                    jObject = (JObject)JToken.ReadFrom(reader);

                }
            }
            var topModel = new ViewModel();
            foreach (var table in jObject)
            {
                topModel.dbTable = table.Key;
                foreach (var attr in JObject.Parse(table.Value.ToString()))
                {
                    topModel.atributes.Add(attr.Value.ToObject<Atribute>());
                }
            }

            int i= 1;
        }
    }
}
