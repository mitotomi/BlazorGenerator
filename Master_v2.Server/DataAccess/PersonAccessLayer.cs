using System;
using Master_v2.Server.Models;
using Master_v2.Shared.Models;
using System.Collections.Generic;

namespace Master_v2.Server.DataAccess
{
	public class PersonAccessLayer
{

		OrderMasterContext _context = new OrderMasterContext();

		public IEnumerable<Person> GetAll()
		{
			try{
				return _context.Person;
			}
			catch(Exception e){
				return null;
			}
		}

		public void Add(Person person){
			try{
				_context.Person.Add(person);
				_context.SaveChanges();
			}
			catch(Exception e){

			}
		}


		public void Update(Person person){
			try{
				if(person != null){
					_context.Person.Update(person);
					_context.SaveChanges();
				}
			}
			catch(Exception e){

			}
		}


		public Person GetById(int id){
			try{
				return _context.Person.Find(id);
			}
			catch (Exception e){
				return null;
			}
		}

		public void Delete(int id){
			Person entity = _context.Person.Find(id);
			if (entity != null){
				_context.Person.Remove(entity);
				_context.SaveChanges();
			}
		}
	}
}
