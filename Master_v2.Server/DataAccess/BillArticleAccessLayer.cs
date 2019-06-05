using System;
using Master_v2.Server.Models;
using Master_v2.Shared.Models;
using System.Collections.Generic;

using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Master_v2.Server.DataAccess
{
	public class BillArticleAccessLayer
{

        OrderMasterContext _context = new OrderMasterContext();

        public List<Article> GetArticle(int id)
		{
			try{
				return _context.BillArticle.Where(x=>x.BillId == id).Include(x=>x.Article).Select(x=>x.Article).Distinct().ToList();
			}
			catch(Exception e){
				return null;
			}
		}

		public List<Bill> GetBill(int id)
		{
			try{
				return _context.BillArticle.Where(x=>x.ArticleId == id).Include(x=>x.Bill).Select(x=>x.Bill).Distinct().ToList();
			}
			catch(Exception e){
				return null;
			}
		}

		public BillArticle GetById(int id1,int id2){
			try{
				return _context.BillArticle.Where(x=>x.BillId==id1 && id2==x.ArticleId).SingleOrDefault();
			}
			catch (Exception e){
				return null;
			}
		}

		public void Delete(int id1, int id2){
			BillArticle entity = _context.BillArticle.Where(x=>x.BillId == id1 && id2 == x.ArticleId).SingleOrDefault();
			if (entity != null){
				_context.BillArticle.Remove(entity);
				_context.SaveChanges();
			}
		}
		public void Add(BillArticle model){
			try{
				BillArticle entity = _context.BillArticle.Where(x=>x.BillId == model.BillId && model.ArticleId == x.ArticleId).SingleOrDefault();
				if (entity == null){
					_context.BillArticle.Add(model);
					_context.SaveChanges();
				}
			}
			catch(Exception e){

			}
		}


		public void Update(BillArticle billarticle){
			try{
				if(billarticle != null){
					_context.BillArticle.Update(billarticle);
					_context.SaveChanges();
				}
			}
			catch(Exception e){

			}
		}


	}
}
