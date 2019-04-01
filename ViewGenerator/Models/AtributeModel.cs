using System;
using System.Collections.Generic;
using System.Text;

namespace ViewGenerator.Models
{
    public class AtributeModel
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool hidden { get; set; }
        public bool validationNeeded { get; set; }
        public bool nullable { get; set; }

        public AtributeModel() { }
    }
}
