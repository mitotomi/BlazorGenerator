using Master_v2.Server.DataAccess;
using Master_v2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master_v2.Server.Controllers 
{
	public class StoreController : Controller 
	{
		StoreAccessLayer _repository=new StoreAccessLayer();
		PersonAccessLayer _repo2 = new PersonAccessLayer ();

		[HttpGet]
		[Route("api/stores")]
		public IEnumerable<Store> Get(){
			return _repository.GetAll();
		}

		[HttpGet]
		[Route("api/store/{id}")]
		public Store GetById(int id){
			if(id==0){
				return new Store();
			}
			else{
				return _repository.GetById(id);
			}
		}

		[HttpPost]
		[Route("api/store/create")]
		public void Post([FromBody] Store model){
			if (ModelState.IsValid){
				_repository.Add(model);
			}
		}
		[HttpPost]
		[Route("api/store/edit")]
		public void Update([FromBody] Store model){
			if (ModelState.IsValid) {
				_repository.Update(model);
			}
		}
		[HttpDelete]
		[Route("api/store/delete/{id}")]
		public void Delete(int id){
			_repository.Delete(id);
		}
        [HttpGet]
        [Route("api/stores/personlastname")]
        public List<SelectListItem> GetPersonLastNameSelectList()
        {
            var all = _repo2.GetAll();
            List<SelectListItem> options = new List<SelectListItem>();
            foreach (var option in all)
            {
                options.Add(new SelectListItem(option.Id, option.LastName));
            }
            return options;
        }
    }
}
