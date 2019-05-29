using System;
using System.Collections.Generic;
using System.Text;

namespace ViewGenerator.Models
{
    public class NNModel
    {
        public string nnTable { get; set; }
        public NNProps nnProps { get; set; }

        public NNModel()
        {
            nnProps = new NNProps();
        }
    }
}
