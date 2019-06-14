using Master_v2.Server.DataAccess;
using Master_v2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master_v2.Server.Controllers 
{
	public class ArticleController : Controller 
	{
		ArticleAccessLayer _repository=new ArticleAccessLayer();

        [HttpGet]
		[Route("api/articles")]
		public IEnumerable<Article> Get(){
            if (!AuthorizationStore.checkReadPermission("article"))
            {
                return new List<Article>();
            }
			return _repository.GetAll();
		}

		[HttpGet]
		[Route("api/article/{id}")]
		public Article GetById(int id){
            if (!AuthorizationStore.checkReadPermission("article"))
            {
                return new Article();
            }
            if (id==0){
				return new Article();
			}
			else{
				return _repository.GetById(id);
			}
		}

		[HttpPost]
		[Route("api/article/create")]
		public void Post([FromBody] Article model){
            if (!AuthorizationStore.checkWritePermissions("article"))
            {
                return;
            }
            if (ModelState.IsValid){
				_repository.Add(model);
			}
		}
		[HttpPost]
		[Route("api/article/edit")]
		public void Update([FromBody] Article model){
            if (!AuthorizationStore.checkWritePermissions("article"))
            {
                return;
            }
            if (ModelState.IsValid) {
				_repository.Update(model);
			}
		}
		[HttpDelete]
		[Route("api/article/delete/{id}")]
		public void Delete(int id){
            if (!AuthorizationStore.checkWritePermissions("article"))
            {
                return ;
            }
            _repository.Delete(id);
		}
	}
}
