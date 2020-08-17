using BLL.Core.Interfaces;
using BLL.Core.Models.Event;
using BLL.Core.Models.Person;
using Constants;
using LumeWebApp.Responses.Person;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace LumeWebApp.Controllers
{
	[Route("api/person")]
	[ApiController]
	public class PersonController : ControllerBase
	{
		private readonly IPersonLogic _personLogic;
		private readonly IPersonValidation _personValidation;
		private readonly IEventValidation _eventValidation;
		private readonly IEventLogic _eventLogic;
		public PersonController(IPersonLogic personLogic,
			IPersonValidation personValidation,
			IEventValidation eventValidation,
			IEventLogic eventLogic)
		{
			_personLogic = personLogic;
			_personValidation = personValidation;
			_eventValidation = eventValidation;
			_eventLogic = eventLogic;
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
		public async Task<ActionResult<PersonModel>> UpdatePerson(UpdatePersonModel request)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _personValidation.ValidateUpdatePerson(request, uid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			return await _personLogic.UpdatePerson(request, uid);
		}

		[HttpPost]
		[Route("remove-person-token")]
		public async Task<ActionResult<PersonModel>> RemovePersonToken()
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			return await _personLogic.RemovePersonToken(uid);
		}

		[HttpPost]
		[Route("get-person-list")]
		public async Task<ActionResult<List<PersonModel>>> GetPersonListByPage(GetPersonListFilter request)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _personValidation.ValidateGetPersonListByPage(request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			var models = await _personLogic.GetPersonListByPage(uid, request);
			return models.ToList();
		}

		[HttpPost]
		[Route("add-feedback")]
		public async Task<ActionResult> AddFeedback(FeedbackModel model)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _personValidation.ValidateFeedback(model);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _personLogic.AddFeedback(model, uid);
			return Ok(Messages.GetMessageJson(MessageTitles.FeedbackAdded, CultureParser.GetCultureFromHttpContext(HttpContext)));
		}

		[HttpGet]
		[Route("get-person-notifications")]
		public async Task<ActionResult<PersonNotificationsModel>> GetPersonNotifications()
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			return await _personLogic.GetPersonNotifications(uid);
		}

		[HttpPost]
		[Route("get-random-person")]
		public async Task<ActionResult<PersonModel>> GetRandomPerson(RandomPersonFilter randomPersonFilter)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _personValidation.ValidateGetRandomPerson(randomPersonFilter);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			var randomEvent = await _personLogic.GetRandomPerson(randomPersonFilter, uid);
			if (randomEvent == null)
			{
				return BadRequest(ErrorDictionary.GetErrorMessage(27, CultureParser.GetCultureFromHttpContext(HttpContext)));
			}
			return randomEvent;
		}

		[HttpPost]
		[Route("accept-random-person")]
		public async Task<ActionResult> AcceptRandomPerson(EventParticipantModel request)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _eventValidation.ValidateAddParticipantModel(request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _eventLogic.AddParticipant(request, uid);
			await _eventLogic.AddEventSwipeHistory(request.PersonUid, request.EventUid);
			return Ok(Messages.GetMessageJson(MessageTitles.RandomPersonAccepted, CultureParser.GetCultureFromHttpContext(HttpContext)));
		}

		[HttpPost]
		[Route("reject-random-person")]
		public async Task<ActionResult> RejectRandomPerson(Guid eventUid, Guid personUid)
		{
			var validationResult = _personValidation.ValidateRejectRandomPerson(eventUid, personUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _eventLogic.AddEventSwipeHistory(personUid, eventUid);
			return Ok(Messages.GetMessageJson(MessageTitles.RandomPersonRejected, CultureParser.GetCultureFromHttpContext(HttpContext)));
		}
	}
}
