using System;
using System.Collections.Generic;
using System.Text;

namespace ViewGenerator.Models
{
    public class TableModel
    {
        public string dbTable { get; set; }
        public List<AtributeModel> atributes { get; set; }

        public TableModel()
        {
            atributes = new List<AtributeModel>();
        }
    }
}
