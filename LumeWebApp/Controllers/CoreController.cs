using BLL.Core.Interfaces;
using BLL.Core.Models;
using Constants;
using LumeWebApp.Requests.Person;
using LumeWebApp.Responses.Event;
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
		private readonly ICoreValidation _coreValidation;

		public CoreController(ICoreLogic coreLogic,
			ICoreValidation coreValidation)
		{
			_coreLogic = coreLogic;
			_coreValidation = coreValidation;
		}

		#region person
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
			var model = new UpdatePersonModel
			{
				PersonUid = uid,
				Age = request.Age,
				Name = request.Name,
				Description = request.Description
			};
			var validationResult = _coreValidation.ValidateUpdatePerson(model);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _coreLogic.UpdatePerson(model);
			return Ok(Messages.PersonUpdateSuccess);
		}
		#endregion person

		#region event
		[HttpPost]
		[Route("add-event")]
		public async Task<ActionResult<AddEventResponse>> AddEvent(AddEventModel request)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _coreValidation.ValidateAddEvent(request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _coreLogic.AddEvent(request, uid);
			return new AddEventResponse();
		}
		#endregion event
	}
}
