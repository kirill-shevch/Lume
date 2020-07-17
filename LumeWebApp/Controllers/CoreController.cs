using BLL.Core.Interfaces;
using BLL.Core.Models;
using Constants;
using LumeWebApp.Requests.Person;
using LumeWebApp.Responses.Person;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LumeWebApp.Controllers
{
	[Route("api/core")]
	[ApiController]
	public class CoreController : ControllerBase
	{
		private readonly ICoreLogic _coreLogic;
		public CoreController(ICoreLogic coreLogic)
		{
			_coreLogic = coreLogic;
		}

		[HttpGet]
		[Route("get-person")]
		public async Task<ActionResult<PersonModel>> GetPerson(Guid? personUid)
		{
			var uid = personUid ?? new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			return await _coreLogic.GetPerson(uid);
		}

		[HttpGet]
		[Route("is-person-filled-up")]
		public async Task<ActionResult<IsPersonFilledUpResponse>> IsPersonFilledUp(Guid? personUid)
		{
			var uid = personUid ?? new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			return new IsPersonFilledUpResponse { IsPersonFilledUp = await _coreLogic.IsPersonFilledUp(uid) };
		}

		[HttpPost]
		[Route("update-person")]
		public async Task<ActionResult> UpdatePerson(UpdatePersonRequest request)
		{
			var uid =  new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			await _coreLogic.UpdatePerson(new UpdatePersonModel { 
				PersonUid = uid, 
				Age = request.Age, 
				Name = request.Name, 
				Description = request.Description 
			});
			return Ok();
		}
	}
}
