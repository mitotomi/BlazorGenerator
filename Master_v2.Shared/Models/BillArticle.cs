using System;
using System.Collections.Generic;

namespace Master_v2.Shared.Models
{
    public partial class BillArticle
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int ArticleId { get; set; }

        public Article Article { get; set; }
        public Bill Bill { get; set; }
    }
}
