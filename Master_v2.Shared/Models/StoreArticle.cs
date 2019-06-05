using System;
using System.Collections.Generic;

namespace Master_v2.Server.Models
{
    public partial class StoreArticle
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int StoreId { get; set; }

        public Article Article { get; set; }
        public Store Store { get; set; }
    }
}
