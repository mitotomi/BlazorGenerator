using Master_v2.Server.DataAccess;
using Master_v2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master_v2.Server.Controllers
{
    public class BillArticleController : Controller
    {
        BillArticleAccessLayer _repository = new BillArticleAccessLayer();
        BillAccessLayer _repo2 = new BillAccessLayer();
        ArticleAccessLayer _repo3 = new ArticleAccessLayer();

        [HttpGet]
        [Route("api/billarticle/article/{id}")]
        public List<Article> GetForBill(int id)
        {
            return _repository.GetArticle(id);
        }

        [HttpGet]
        [Route("api/billarticle/bill/{id}")]
        public List<Bill> GetForArticle(int id)
        {
            return _repository.GetBill(id);
        }

        [HttpGet]
        [Route("api/billarticle/{id1}/{id2}")]
        public BillArticle GetById(int id1, int id2)
        {
            if (id1 == 0 || id2 == 0)
            {
                var ret = new BillArticle();
                ret.BillId = id1;
                ret.ArticleId = id2;
                return ret;
            }
            else
            {
                return _repository.GetById(id1, id2);
            }
        }

        [HttpPost]
        [Route("api/billarticle/create")]
        public void Post([FromBody] BillArticle model)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(model);
            }
        }
        [HttpPost]
        [Route("api/billarticle/edit")]
        public void Update([FromBody] BillArticle model)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(model);
            }
        }
        [HttpDelete]
        [Route("api/billarticle/delete/{id1}/{id2}")]
        public void Delete(int id1, int id2)
        {
            _repository.Delete(id1, id2);
        }
        [HttpGet]
        [Route("api/billarticles/billdate")]
        public List<SelectListItem> GetBillDateSelectList()
        {
            var all = _repo2.GetAll();
            List<SelectListItem> options = new List<SelectListItem>();
            foreach (var option in all)
            {
                options.Add(new SelectListItem(option.Id, option.Date.ToString()));
            }
            return options;
        }
        [HttpGet]
        [Route("api/billarticles/articlename")]
        public List<SelectListItem> GetArticleNameSelectList()
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
