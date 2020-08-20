using BLL.Authorization.Interfaces;
using BLL.Core.Interfaces;
using BLL.Core.Models.Person;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LumeWebApp.Controllers
{
	[Route("api/contacts")]
	[ApiController]
	public class ContactsController : ControllerBase
	{
		private readonly IAuthorizationLogic _authorizationLogic;
		private readonly IPersonLogic _personLogic;
		public ContactsController(IAuthorizationLogic authorizationLogic,
			IPersonLogic personLogic)
		{
			_authorizationLogic = authorizationLogic;
			_personLogic = personLogic;
		}
		
		[HttpPost]
		[Route("get-person-list-by-contacts")]
		public async Task<ActionResult<List<PersonModel>>> GetPersonListByContacts(List<string> phoneNumbers)
		{
			var personUids = await _authorizationLogic.GetPersonListByContacts(phoneNumbers);
			var personModelList = new List<PersonModel>();
			foreach (var uid in personUids)
			{
				var model = await _personLogic.GetPerson(uid);
				personModelList.Add(model);
			}
			return personModelList;
		}
	}
}
