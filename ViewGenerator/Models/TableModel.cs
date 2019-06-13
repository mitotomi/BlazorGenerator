using System;
using System.Collections.Generic;
using System.Text;

namespace ViewGenerator.Models
{
    public class TableModel
    {
        public string dbTable { get; set; }
        public int[] readPermissions { get; set; }
        public int[] writePermissions { get; set; }
        public List<AtributeModel> atributes { get; set; }
        public List<ChildModel> children { get; set; }
        public List<NNRelationModel> nNRelations { get; set; }

        public TableModel()
        {
            atributes = new List<AtributeModel>();
            children = new List<ChildModel>();
            nNRelations = new List<NNRelationModel>();
            readPermissions = new int[0];
            writePermissions = new int[0];
        }
    }
}
