using System;
using System.Collections.Generic;

namespace Master_v2.Shared.Models
{
    public partial class Bill
    {
        public Bill()
        {
            BillArticle = new HashSet<BillArticle>();
        }

        public int Id { get; set; }
        public int PersonId { get; set; }
        public DateTime Date { get; set; }
        public int StoreId { get; set; }

        public Person Person { get; set; }
        public Store Store { get; set; }
        public ICollection<BillArticle> BillArticle { get; set; }
    }
}
