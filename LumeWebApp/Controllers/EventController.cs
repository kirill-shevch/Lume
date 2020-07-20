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
	}
}
