using Master_v2.Server.DataAccess;
using Master_v2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master_v2.Server.Controllers
{
    public class BillController : Controller
    {
        BillAccessLayer _repository = new BillAccessLayer();
        PersonAccessLayer _repo2 = new PersonAccessLayer();
        StoreAccessLayer _repo3 = new StoreAccessLayer();

        [HttpGet]
        [Route("api/bills")]
        public IEnumerable<Bill> Get()
        {

            return _repository.GetAll();
        }

        [HttpGet]
        [Route("api/bill/{id}")]
        public Bill GetById(int id)
        {
            if (id == 0)
            {
                return new Bill();
            }
            else
            {
                return _repository.GetById(id);
            }
        }

        [HttpPost]
        [Route("api/bill/create")]
        public void Post([FromBody] Bill model)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(model);
            }
        }
        [HttpPost]
        [Route("api/bill/edit")]
        public void Update([FromBody] Bill model)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(model);
            }
        }
        [HttpDelete]
        [Route("api/bill/delete/{id}")]
        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        [HttpGet]
        [Route("api/bills/personlastname")]
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
        [HttpGet]
        [Route("api/bills/storename")]
        public List<SelectListItem> GetStoreNameSelectList()
        {
            var all = _repo3.GetAll();
            List<SelectListItem> options = new List<SelectListItem>();
            foreach (var option in all)
            {
                options.Add(new SelectListItem(option.Id, option.Name));
            }
            return options;
        }
    }
}
