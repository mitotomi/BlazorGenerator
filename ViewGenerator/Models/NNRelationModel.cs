using System;
using System.Collections.Generic;
using System.Text;

namespace ViewGenerator.Models
{
    public class NNRelationModel
    {
        public string relationTable { get; set; }
        public string mainAttribute { get; set; }
        public string endAttribute { get; set; }
        public ChildModel endTable { get; set; }

        public NNRelationModel()
        {
            endTable = new ChildModel();
        }
    }
}
