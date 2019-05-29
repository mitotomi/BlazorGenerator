﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ViewGenerator.Models
{
    public class NNRelationModel
    {
        public string nnTable { get; set; }
        public List<AtributeModel> atributes { get; set; }

        public NNRelationModel()
        {
            atributes = new List<AtributeModel>();
        }
    }
}
