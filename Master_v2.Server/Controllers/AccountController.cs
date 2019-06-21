using System.Linq;
using Master_v2.Server.Models;
using Master_v2.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Master_v2.Server.Controllers
{
	public class AccountController: Controller
	{
		OrderMasterContext _context = new OrderMasterContext();

		[HttpPost]
		[Route("api/login")]
		public IActionResult LogIn([FromBody] UserModel model)
		{
			var user = _context.Person.Where(x=>model.password==x.Password && model.userName==x.UserName).SingleOrDefault();
			if(user != null)
		{
				AuthorizationStore.setRoleId(user.RoleId);
				return new ObjectResult(user.RoleId);
			}
			return new ObjectResult(0);
		}

		[HttpPost]
		[Route("api/register")]
		public IActionResult Register([FromBody] Person model)
		{
			if(ModelState.IsValid)
			{
				_context.Person.Add(model);
				_context.SaveChanges();
				AuthorizationStore.setRoleId(model.RoleId);
				return new ObjectResult(model.RoleId);
			}
			else
			{
				return new ObjectResult(0);
			}

		}
	}
}
