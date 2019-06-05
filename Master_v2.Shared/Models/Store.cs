using System;
using System.Collections.Generic;

namespace Master_v2.Server.Models
{
    public partial class Store
    {
        public Store()
        {
            Bill = new HashSet<Bill>();
            StoreArticle = new HashSet<StoreArticle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int OwnerId { get; set; }

        public Person Owner { get; set; }
        public ICollection<Bill> Bill { get; set; }
        public ICollection<StoreArticle> StoreArticle { get; set; }
    }
}
