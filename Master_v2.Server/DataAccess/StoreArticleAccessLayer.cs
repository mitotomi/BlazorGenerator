using System;
using Master_v2.Server.Models;
using Master_v2.Shared.Models;
using System.Collections.Generic;

using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Master_v2.Server.DataAccess
{
	public class StoreArticleAccessLayer
{

        OrderMasterContext _context = new OrderMasterContext();

        public List<Article> GetArticle(int id)
		{
			try{
				return _context.StoreArticle.Where(x=>x.StoreId == id).Include(x=>x.Article).Select(x=>x.Article).Distinct().ToList();
			}
			catch(Exception e){
				return null;
			}
		}

		public List<Store> GetStore(int id)
		{
			try{
				return _context.StoreArticle.Where(x=>x.ArticleId == id).Include(x=>x.Store).Select(x=>x.Store).Distinct().ToList();
			}
			catch(Exception e){
				return null;
			}
		}

		public void Delete(int id1, int id2){
			StoreArticle entity = _context.StoreArticle.Where(x=>x.StoreId == id1 && id2 == x.ArticleId).SingleOrDefault();
			if (entity != null){
				_context.StoreArticle.Remove(entity);
				_context.SaveChanges();
			}
		}
		public void Add(StoreArticle storearticle){
			try{
				_context.StoreArticle.Add(storearticle);
				_context.SaveChanges();
			}
			catch(Exception e){

			}
		}


		public void Update(StoreArticle storearticle){
			try{
				if(storearticle != null){
					_context.StoreArticle.Update(storearticle);
					_context.SaveChanges();
				}
			}
			catch(Exception e){

			}
		}


	}
}
