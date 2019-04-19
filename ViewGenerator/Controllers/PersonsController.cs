using Master_v2.Server.DataAccess;
using Master_v2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master_v2.Server.Controllers 
{
	public class PersonsController : Controller 
	{
		Repo _repository=new Repo();
		[HttpGet]
		[Route("api/persons")]
		public IEnumerable<Person> Get(){
			return _repository.GetAll();
		}

		[HttpGet]
		[Route("api/person/{id}")]
		public IEnumerable<Person> GetById(int id){
			return _repository.GetById(id);
		}

		[HttpPost]
		[Route("api/person/create")]
		public void Post([FromBody] Person model){
			if (ModelState.IsValid){
				_repository.Add(model);
			}
		}
		[HttpPost]
		[Route("api/person/edit")]
		public void Update([FromBody] Person model){
			if (ModelState.IsValid) {
				_repository.Update(model);
			}
		}
		[HttpDelete]
		[Route("api/person/delete/{id}")]
		public void Delete(int id){
			_repository.Delete(id);
		}
	}
}
