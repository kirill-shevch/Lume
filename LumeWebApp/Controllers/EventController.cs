using BLL.Core.Interfaces;
using BLL.Core.Models.Event;
using Constants;
using LumeWebApp.Responses.Event;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace LumeWebApp.Controllers
{
	[Route("api/event")]
	[ApiController]
	public class EventController : ControllerBase
	{
		private readonly IEventLogic _eventLogic;
		private readonly IEventValidation _eventValidation;
		private readonly IPersonLogic _personLogic;
		public EventController(IEventLogic eventLogic,
			IPersonLogic personLogic,
			IEventValidation eventValidation)
		{
			_eventLogic = eventLogic;
			_eventValidation = eventValidation;
			_personLogic = personLogic;
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
		public async Task<ActionResult<GetEventModel>> UpdateEvent(UpdateEventModel request)
		{
			var validationResult = _eventValidation.ValidateUpdateEvent(request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			return await _eventLogic.UpdateEvent(request);
		}

		[HttpPost]
		[Route("add-promo-reward-request")]
		public async Task<ActionResult> AddPromoRewardRequest(PromoRewardRequestModel request)
		{
			var validationResult = _eventValidation.ValidateAddPromoRewardRequest(request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _eventLogic.AddPromoRewardRequest(request);
			return Ok(Messages.GetMessageJson(MessageTitles.PromoRewardRequestAdded, CultureParser.GetCultureFromHttpContext(HttpContext)));
		}

		[HttpDelete]
		[Route("remove-event-image")]
		public async Task<ActionResult> RemoveEventImage(RemoveEventImageModel request)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _eventValidation.ValidateRemoveEventImage(uid, request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _eventLogic.RemoveEventImage(request);
			return Ok(Messages.GetMessageJson(MessageTitles.EventImageRemoved, CultureParser.GetCultureFromHttpContext(HttpContext)));
		}

		[HttpPost]
		[Route("add-event-participant")]
		public async Task<ActionResult<GetEventModel>> AddEventParticipant(EventParticipantModel request)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _eventValidation.ValidateAddParticipantModel(request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			return await _eventLogic.AddParticipant(request, uid);
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
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _eventValidation.ValidateRemoveEventParticipant(personUid, eventUid, uid);
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
				return BadRequest(ErrorDictionary.GetErrorMessage(25, CultureParser.GetCultureFromHttpContext(HttpContext)));
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
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _eventValidation.ValidateAddParticipantModel(request);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _eventLogic.AddParticipant(request, uid);
			await _personLogic.AddPersonSwipeHistory(request.EventUid, request.PersonUid);
			return Ok(Messages.GetMessageJson(MessageTitles.RandomEventAccepted, CultureParser.GetCultureFromHttpContext(HttpContext)));
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
			await _personLogic.AddPersonSwipeHistory(eventUid, uid);
			return Ok(Messages.GetMessageJson(MessageTitles.RandomEventRejected, CultureParser.GetCultureFromHttpContext(HttpContext)));
		}

		[HttpPost]
		[Route("add-report")]
		public async Task<ActionResult> AddReport(EventReportModel model)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _eventValidation.ValidateAddReport(model);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _eventLogic.AddReport(model, uid);
			return Ok(Messages.GetMessageJson(MessageTitles.ReportAdded, CultureParser.GetCultureFromHttpContext(HttpContext)));
		}
	}
}
