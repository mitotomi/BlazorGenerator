using System;
using System.Collections.Generic;
using System.Text;

namespace Master_v2.Shared.Models
{
    public class SelectListItem
    {
        public int Key { get; set; }
        public string Value { get; set; }

        public SelectListItem() { }
        public SelectListItem(int id, string text)
        {
            Key = id;
            Value = text;
        }
    }
}
