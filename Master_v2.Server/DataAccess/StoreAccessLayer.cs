using System;
using Master_v2.Server.Models;
using Master_v2.Shared.Models;
using System.Collections.Generic;

using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Master_v2.Server.DataAccess
{
	public class StoreAccessLayer
{

        OrderMasterContext _context = new OrderMasterContext();

		public IEnumerable<Store> GetAll()
		{
			try{
				return _context.Store;
			}
			catch(Exception e){
				return null;
			}
		}

		public void Add(Store store){
			try{
				_context.Store.Add(store);
				_context.SaveChanges();
			}
			catch(Exception e){

			}
		}


		public void Update(Store store){
			try{
				if(store != null){
					_context.Store.Update(store);
					_context.SaveChanges();
				}
			}
			catch(Exception e){

			}
		}


		public Store GetById(int id){
			try{
				return _context.Store.Find(id);
			}
			catch (Exception e){
				return null;
			}
		}

		public void Delete(int id){
			Store entity = _context.Store.Find(id);
			if (entity != null){
				_context.Store.Remove(entity);
				_context.SaveChanges();
			}
		}
	}
}
