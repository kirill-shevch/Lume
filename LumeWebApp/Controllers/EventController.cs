using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Core.Interfaces;
using BLL.Core.Models.Event;
using Constants;
using LumeWebApp.Responses.Event;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LumeWebApp.Controllers
{
	[Route("api/event")]
	[ApiController]
	public class EventController : ControllerBase
	{
		private readonly IEventLogic _eventLogic;
		private readonly IEventValidation _eventValidation;

		public EventController(IEventLogic eventLogic,
			IEventValidation eventValidation)
		{
			_eventLogic = eventLogic;
			_eventValidation = eventValidation;
		}

		[HttpPost]
		[Route("add-event")]
		public async Task<ActionResult<AddEventResponse>> AddEvent(AddEventModel request)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _eventValidation.ValidateAddEvent(request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			var eventUid = await _eventLogic.AddEvent(request, uid);
			return new AddEventResponse { EventUid = eventUid };
		}

		[HttpGet]
		[Route("get-event")]
		public async Task<ActionResult<GetEventModel>> GetEvent(Guid eventUid)
		{
			var validationResult = _eventValidation.ValidateGetEvent(eventUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			return await _eventLogic.GetEvent(eventUid);
		}

		[HttpGet]
		[Route("get-event-list")]
		public async Task<ActionResult<List<GetEventListModel>>> GetEventList()
		{
			var personUid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			return await _eventLogic.GetEventList(personUid);
		}

		[HttpPost]
		[Route("update-event")]
		public async Task<ActionResult> UpdateEvent(UpdateEventModel request)
		{
			var validationResult = _eventValidation.ValidateUpdateEvent(request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _eventLogic.UpdateEvent(request);
			return Ok(Messages.UpdateSuccess);
		}

		[HttpPost]
		[Route("add-event-participant")]
		public async Task<ActionResult> AddEventParticipant(EventParticipantModel request)
		{
			var validationResult = _eventValidation.ValidateAddParticipantModel(request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _eventLogic.AddParticipant(request);
			return Ok(Messages.ParticipantCreated);
		}

		[HttpPost]
		[Route("update-event-participant")]
		public async Task<ActionResult<GetEventModel>> UpdateEventParticipant(EventParticipantModel request)
		{
			var validationResult = _eventValidation.ValidateUpdateParticipantModel(request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _eventLogic.UpdateParticipant(request);
			return await _eventLogic.GetEvent(request.EventUid);
		}

		[HttpDelete]
		[Route("remove-event-participant")]
		public async Task<ActionResult<GetEventModel>> RemoveEventParticipant(Guid personUid, Guid eventUid)
		{
			var validationResult = _eventValidation.ValidateRemoveEventParticipant(personUid, eventUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _eventLogic.RemoveParticipant(personUid, eventUid);
			return await _eventLogic.GetEvent(eventUid);
		}

		[HttpPost]
		[Route("get-random-event")]
		public async Task<ActionResult<GetEventModel>> GetRandomEvent(RandomEventFilter randomEventFilter)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _eventValidation.ValidateGetRandomEvent(randomEventFilter);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			var randomEvent = await _eventLogic.GetRandomEvent(randomEventFilter, uid);
			if (randomEvent == null)
			{
				return BadRequest(ErrorDictionary.GetErrorMessage(25));
			}
			return randomEvent;
		}

		[HttpPost]
		[Route("search-for-event")]
		public async Task<ActionResult<List<GetEventListModel>>> SearchForEvent(EventSearchFilter eventSearchFilter)
		{
			var validationResult = _eventValidation.ValidateSearchForEvent(eventSearchFilter);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			return await _eventLogic.SearchForEvent(eventSearchFilter);
		}

		[HttpPost]
		[Route("accept-random-event")]
		public async Task<ActionResult> AcceptRandomEvent(EventParticipantModel request)
		{
			var validationResult = _eventValidation.ValidateAddParticipantModel(request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _eventLogic.AddParticipant(request);
			await _eventLogic.AddEventSwipeHistory(request.EventUid, request.PersonUid);
			return Ok(Messages.RandomEventAccepted);
		}

		[HttpPost]
		[Route("reject-random-event")]
		public async Task<ActionResult> RejectRandomEvent(Guid eventUid)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _eventValidation.ValidateGetEvent(eventUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _eventLogic.AddEventSwipeHistory(eventUid, uid);
			return Ok(Messages.RandomEventRejected);
		}
	}
}
