using System;
using Master_v2.Server.Models;
using Master_v2.Shared.Models;
using System.Collections.Generic;

using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Master_v2.Server.DataAccess
{
	public class ArticleAccessLayer
{

		OrderMasterContext _context = new OrderMasterContext();

		public IEnumerable<Article> GetAll()
		{
			try{
				return _context.Article;
			}
			catch(Exception e){
				return null;
			}
		}

		public void Add(Article article){
			try{
				_context.Article.Add(article);
				_context.SaveChanges();
			}
			catch(Exception e){

			}
		}


		public void Update(Article article){
			try{
				if(article != null){
					_context.Article.Update(article);
					_context.SaveChanges();
				}
			}
			catch(Exception e){

			}
		}


		public Article GetById(int id){
			try{
				return _context.Article.Find(id);
			}
			catch (Exception e){
				return null;
			}
		}

		public void Delete(int id){
			Article entity = _context.Article.Find(id);
			if (entity != null){
				_context.Article.Remove(entity);
				_context.SaveChanges();
			}
		}
	}
}
