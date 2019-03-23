using System;
using System.Collections.Generic;
using System.Text;

namespace ViewGenerator
{
    public class Atribute
    {
        public string name { get; set; }
        public string value {get; set;}
        public string type { get; set; }
        public bool hidden { get; set; }
        public bool validationNeeded { get; set; }
        public bool nullable { get; set; }

        public Atribute() { }
    }
}
