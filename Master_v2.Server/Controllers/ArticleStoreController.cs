using Master_v2.Server.DataAccess;
using Master_v2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master_v2.Server.Controllers
{
    public class ArticleStoreController : Controller
    {
        ArticleStoreAccessLayer _repository = new ArticleStoreAccessLayer();

        [HttpGet]
        [Route("api/stores/articles/{id}")]
        public List<Article> GetAllArticles(int id)
        {
            return _repository.GetArticles(id);
        }

        [HttpGet]
        [Route("api/articles/stores/{id}")]
        public List<Store> GetAllStores(int id)
        {
            return _repository.GetStores(id);
        }

        [HttpPost]
        [Route("api/stores/article/create")]
        public void Post([FromBody]StoreArticle storeArticle)
        {
            if (ModelState.IsValid)
            {
                _repository.PostStoreArticle(storeArticle);
            }
        }

        [HttpPost]
        [Route("api/article/store/update")]
        public void Update([FromBody]StoreArticle storeArticle)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateStoreArticle(storeArticle);
            }
        }
    }
}
