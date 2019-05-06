using System;
using System.Collections.Generic;
using System.Text;

namespace ViewGenerator.Models
{
    public class AtributeModel
    {
        /// <summary>
        /// name is name of property in table, type is type of razor element, hidden if this prop should be hidden
        /// if there is any validation then validationNeeded should be true so that generator renders validation
        /// nullable prop is for create and update forms
        /// </summary>
        public string name { get; set; }
        public string type { get; set; }
        public bool hidden { get; set; }
        public bool validationNeeded { get; set; }
        public bool nullable { get; set; }

        public AtributeModel() { }
    }
}
