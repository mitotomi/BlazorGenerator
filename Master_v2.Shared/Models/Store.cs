using System;
using System.Collections.Generic;

namespace Master_v2.Shared.Models
{
    public partial class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int OwnerId { get; set; }

        public Person Owner { get; set; }
    }
}
