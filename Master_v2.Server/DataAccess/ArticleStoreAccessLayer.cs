using Master_v2.Server.Models;
using Master_v2.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master_v2.Server.DataAccess
{
    public class ArticleStoreAccessLayer
    {
        OrderMasterContext _context = new OrderMasterContext();

        public List<Article> GetArticles(int storeId)
        {
            var existingArticles = _context.StoreArticle.Where(x => x.StoreId == storeId).Include(x => x.Article).Select(x => x.Article).Distinct().ToList();
            return existingArticles;
        }

        public List<Store> GetStores(int articleId)
        {
            var existingStores = _context.StoreArticle.Where(x => x.ArticleId == articleId).Include(x => x.Store).Select(x => x.Store).ToList();
            return existingStores;
        }

        public void DeleteArticleStore(int articleId, int storeId)
        {
            var existing = _context.StoreArticle.Where(x=>x.StoreId == storeId && x.ArticleId==articleId).SingleOrDefault();
            if (existing != null)
            {
                _context.StoreArticle.Remove(existing);
                _context.SaveChanges();
            }
        }
        public void PostStoreArticle(StoreArticle storeArticle)
        {
            _context.StoreArticle.Add(storeArticle);
            _context.SaveChanges();
        }

        public void UpdateStoreArticle(StoreArticle storeArticle)
        {
            try
            {
                if (storeArticle != null)
                {
                    _context.StoreArticle.Update(storeArticle);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
