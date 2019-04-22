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
        public IActionResult Get()
        {
            var data = _repository.GetAll();
            return View("PersonTable",data);
        }

        [HttpGet]
        [Route("api/person/{id}")]
        public IActionResult GetById(int id)
        {
            if (id == 0) { return View("Get"); }
            var model = _repository.GetById(id);
            return View("Person", model);
        }

        [HttpGet]
        [Route("api/person/{id}")]
        public IActionResult GetBy(int id)
        {
            if (id == 0) { var model2 = new Person(); return View("PersonCreate", model2); }
            var model = _repository.GetById(id);
            return View("PersonCreate", model);
        }

        [HttpPost]
        [Route("api/person/create")]
        public IActionResult Post([FromBody] Person model)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(model);
                return RedirectToAction(nameof(PersonController.Get));
            }
            return RedirectToAction(nameof(PersonController.GetBy));

        }
        [HttpPost]
        [Route("api/person/edit")]
        public IActionResult Update([FromBody] Person model)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(model);
                return RedirectToAction(nameof(PersonController.Get));
            }
            return RedirectToAction(nameof(PersonController.GetBy));

        }
        [HttpDelete]
        [Route("api/person/delete/{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return RedirectToAction(nameof(PersonController.Get));
        }
    }
}
