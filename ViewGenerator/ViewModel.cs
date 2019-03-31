using System;
using System.Collections.Generic;
using System.Text;

namespace ViewGenerator
{
    class ViewModel
    {
        public string dbTable { get; set; }
        public List<Atribute> atributes { get; set; }

        public ViewModel()
        {
            atributes = new List<Atribute>();
        }
    }
}
