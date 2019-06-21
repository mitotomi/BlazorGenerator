using Master_v2.Server.DataAccess;
using Master_v2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master_v2.Server.Controllers
{
    public class StoreArticleController : Controller
    {
        StoreArticleAccessLayer _repository = new StoreArticleAccessLayer();
        StoreAccessLayer _repo2 = new StoreAccessLayer();
        ArticleAccessLayer _repo3 = new ArticleAccessLayer();

        [HttpGet]
        [Route("api/storearticle/article/{id}")]
        public List<Article> GetForStore(int id)
        {
            return _repository.GetArticle(id);
        }

        [HttpGet]
        [Route("api/storearticle/store/{id}")]
        public List<Store> GetForArticle(int id)
        {
            return _repository.GetStore(id);
        }

        [HttpGet]
        [Route("api/storearticle/{id1}/{id2}")]
        public StoreArticle GetById(int id1, int id2)
        {
            if (id1 == 0 || id2 == 0)
            {
                var ret = new StoreArticle();
                ret.StoreId = id1;
                ret.ArticleId = id2;
                return ret;
            }
            else
            {
                return _repository.GetById(id1, id2);
            }
        }

        [HttpPost]
        [Route("api/storearticle/create")]
        public void Post([FromBody] StoreArticle model)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(model);
            }
        }
        [HttpPost]
        [Route("api/storearticle/edit")]
        public void Update([FromBody] StoreArticle model)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(model);
            }
        }
        [HttpDelete]
        [Route("api/storearticle/delete/{id1}/{id2}")]
        public void Delete(int id1, int id2)
        {
            _repository.Delete(id1, id2);
        }
        [HttpGet]
        [Route("api/storearticles/storename")]
        public List<SelectListItem> GetStoreNameSelectList()
        {
            var all = _repo2.GetAll();
            List<SelectListItem> options = new List<SelectListItem>();
            foreach (var option in all)
            {
                options.Add(new SelectListItem(option.Id, option.Name));
            }
            return options;
        }
        [HttpGet]
        [Route("api/storearticles/articlename")]
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
