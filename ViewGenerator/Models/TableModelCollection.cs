using System;
using System.Collections.Generic;
using System.Text;

namespace ViewGenerator.Models
{
    public class TableModelCollection
    {
        public List<TableModel> tableModels { get; set; }

        public TableModelCollection()
        {
            tableModels = new List<TableModel>();
        }
    }
}
