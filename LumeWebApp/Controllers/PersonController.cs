using BLL.Core.Interfaces;
using BLL.Core.Models.Person;
using Constants;
using LumeWebApp.Requests.Person;
using LumeWebApp.Responses.Person;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LumeWebApp.Controllers
{
	[Route("api/person")]
	[ApiController]
	public class PersonController : ControllerBase
	{
		private readonly IPersonLogic _personLogic;
		private readonly IPersonValidation _personValidation;
		public PersonController(IPersonLogic personLogic,
			IPersonValidation personValidation)
		{
			_personLogic = personLogic;
			_personValidation = personValidation;
		}

		[HttpGet]
		[Route("get-profile")]
		public async Task<ActionResult<PersonModel>> GetProfile()
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			return await _personLogic.GetPerson(uid);
		}

		[HttpGet]
		[Route("get-person")]
		public async Task<ActionResult<PersonModel>> GetPerson(Guid personUid)
		{
			var currentPersonUid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _personValidation.ValidateGetPerson(personUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			var model = await _personLogic.GetPerson(personUid);
			model.IsFriend = await _personLogic.CheckFriendship(currentPersonUid, personUid);
			return model;
		}

		[HttpGet]
		[Route("is-person-filled-up")]
		public async Task<ActionResult<IsPersonFilledUpResponse>> IsPersonFilledUp(Guid? personUid)
		{
			var uid = personUid ?? new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			return new IsPersonFilledUpResponse { IsPersonFilledUp = await _personLogic.IsPersonFilledUp(uid) };
		}

		[HttpPost]
		[Route("update-person")]
		public async Task<ActionResult> UpdatePerson(UpdatePersonRequest request)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var model = new UpdatePersonModel
			{
				PersonUid = uid,
				Age = request.Age,
				Name = request.Name,
				Description = request.Description
			};
			var validationResult = _personValidation.ValidateUpdatePerson(model);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _personLogic.UpdatePerson(model);
			return Ok(Messages.UpdateSuccess);
		}

		[HttpGet]
		[Route("get-person-list")]
		public async Task<ActionResult<List<PersonModel>>> GetPersonListByPage(int pageNumber, int pageSize, string filter)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var models = await _personLogic.GetPersonListByPage(new GetPersonListModel
			{ 
				PersonUid = uid, 
				PageNumber = pageNumber, 
				PageSize = pageSize, 
				Filter = filter });
			return models.ToList();
		}
	}
}
