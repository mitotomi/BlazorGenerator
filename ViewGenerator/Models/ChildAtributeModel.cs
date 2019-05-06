using System;
using System.Collections.Generic;
using System.Text;

namespace ViewGenerator.Models
{
    public class ChildAtributeModel
    {
        /// <summary>
        /// name, type, hidden and nullable are just like at attribute model
        /// foreignKey is bool if attr is foreign key,if it is true then it should be known what 
        /// table is it from and what props from it are for select
        /// </summary>
        public string name { get; set; }
        public string type { get; set; }
        public bool hidden { get; set; }
        public bool foreignKey { get; set; }
        public string fkTable { get; set; }
        public string fkValue { get; set; }
        public bool nullable { get; set; }

    }
}
