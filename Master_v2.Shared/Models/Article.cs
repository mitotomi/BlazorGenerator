using System;
using System.Collections.Generic;

namespace Master_v2.Shared.Models
{
    public partial class Article
    {
        public Article()
        {
            BillArticle = new HashSet<BillArticle>();
            StoreArticle = new HashSet<StoreArticle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BillArticle> BillArticle { get; set; }
        public ICollection<StoreArticle> StoreArticle { get; set; }
    }
}
