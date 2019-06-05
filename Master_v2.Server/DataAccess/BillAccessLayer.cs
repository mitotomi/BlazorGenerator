using System;
using Master_v2.Server.Models;
using Master_v2.Shared.Models;
using System.Collections.Generic;

using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Master_v2.Server.DataAccess
{
	public class BillAccessLayer
{

        OrderMasterContext _context = new OrderMasterContext();

        public IEnumerable<Bill> GetAll()
		{
			try{
				return _context.Bill;
			}
			catch(Exception e){
				return null;
			}
		}

		public void Add(Bill bill){
			try{
				_context.Bill.Add(bill);
				_context.SaveChanges();
			}
			catch(Exception e){

			}
		}


		public void Update(Bill bill){
			try{
				if(bill != null){
					_context.Bill.Update(bill);
					_context.SaveChanges();
				}
			}
			catch(Exception e){

			}
		}


		public Bill GetById(int id){
			try{
				return _context.Bill.Find(id);
			}
			catch (Exception e){
				return null;
			}
		}

		public void Delete(int id){
			Bill entity = _context.Bill.Find(id);
			if (entity != null){
				_context.Bill.Remove(entity);
				_context.SaveChanges();
			}
		}
	}
}
