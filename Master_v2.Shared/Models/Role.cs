using System;
using System.Collections.Generic;

namespace Master_v2.Shared.Models
{
    public partial class Role
    {
        public Role()
        {
            Person = new HashSet<Person>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Person> Person { get; set; }
    }
}
