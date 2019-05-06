using System;
using System.Collections.Generic;
using System.Text;

namespace ViewGenerator.Models
{
    public class ChildModel
    {
        public string dbTable { get; set; }
        public List<AtributeModel> atributes { get; set; }

        public ChildModel()
        {
            atributes = new List<AtributeModel>();
        }
    }
}
