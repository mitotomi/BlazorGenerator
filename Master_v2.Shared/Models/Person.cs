﻿using System;
using System.Collections.Generic;

namespace Master_v2.Shared.Models
{
    public partial class Person
    {
        public Person()
        {
            Bill = new HashSet<Bill>();
            Store = new HashSet<Store>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Oib { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public Role Role { get; set; }
        public ICollection<Bill> Bill { get; set; }
        public ICollection<Store> Store { get; set; }
    }
}
