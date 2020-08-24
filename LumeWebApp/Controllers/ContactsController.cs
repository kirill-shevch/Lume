using BLL.Authorization.Interfaces;
using BLL.Core.Interfaces;
using BLL.Core.Models.Person;
using Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var personUids = await _authorizationLogic.GetPersonListByContacts(phoneNumbers);
			var personModelList = await _personLogic.GetPersonList(personUids, uid);
			return personModelList;
		}
	}
}
