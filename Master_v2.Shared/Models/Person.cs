using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Master_v2.Shared.Models
{
    public partial class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Oib { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
    }
}
