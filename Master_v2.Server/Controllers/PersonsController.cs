using Master_v2.Server.DataAccess;
using Master_v2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master_v2.Server.Controllers
{
    public class PersonController : Controller
    {
        PersonAccessLayer _repository = new PersonAccessLayer();

        [HttpGet]
        [Route("api/persons")]
        public IEnumerable<Person> Get()
        {

            return _repository.GetAll();
        }

        [HttpGet]
        [Route("api/person/{id}")]
        public Person GetById(int id)
        {
            if (id == 0)
            {
                return new Person();
            }
            else
            {
                return _repository.GetById(id);
            }
        }

        [HttpPost]
        [Route("api/person/create")]
        public void Post([FromBody] Person model)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(model);
            }
        }
        [HttpPost]
        [Route("api/person/edit")]
        public void Update([FromBody] Person model)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(model);
            }
        }
        [HttpDelete]
        [Route("api/person/delete/{id}")]
        public void Delete(int id)
        {
            _repository.Delete(id);
        }
        [HttpGet]
        [Route("api/person/store/{id}")]
        public List<Store> GetStore(int id)
        {
            return _repository.GetStoreChildren(id);
        }

        [HttpGet]
        [Route("api/person/bill/{id}")]
        public List<Bill> GetBill(int id)
        {
            return _repository.GetBillChildren(id);
        }

    }
}
