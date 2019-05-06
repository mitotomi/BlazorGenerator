using System;
using System.Collections.Generic;

namespace Master_v2.Shared.Models
{
    public partial class Person
    {
        public Person()
        {
            Store = new HashSet<Store>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Oib { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }

        public ICollection<Store> Store { get; set; }
    }
}
