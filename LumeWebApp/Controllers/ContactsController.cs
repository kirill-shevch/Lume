using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Authorization.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LumeWebApp.Controllers
{
	[Route("api/contacts")]
	[ApiController]
	public class ContactsController : ControllerBase
	{
		private readonly IAuthorizationLogic _authorizationLogic;
		public ContactsController(IAuthorizationLogic authorizationLogic)
		{
			_authorizationLogic = authorizationLogic;
		}
		
		[HttpPost]
		[Route("get-person-list-by-contacts")]
		public async Task<ActionResult<List<Guid>>> GetPersonListByContacts(List<string> phoneNumbers)
		{
			return await _authorizationLogic.GetPersonListByContacts(phoneNumbers);
		}
	}
}
