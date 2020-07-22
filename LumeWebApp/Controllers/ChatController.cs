using BLL.Core.Interfaces;
using BLL.Core.Models.Chat;
using Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LumeWebApp.Controllers
{
	[Route("api/chat")]
	[ApiController]
	public class ChatController : ControllerBase
	{
		private readonly IChatValidation _chatValidation;
		private readonly IChatLogic _chatLogic;

		public ChatController(IChatValidation chatValidation,
			IChatLogic chatLogic)
		{
			_chatValidation = chatValidation;
			_chatLogic = chatLogic;
		}


		[HttpGet]
		[Route("get-chat")]
		public async Task<ActionResult<ChatModel>> GetChat(Guid chatUid, uint pageNumber, uint pageSize) 
		{
			var validationResult = _chatValidation.ValidateGetChat(chatUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			return await _chatLogic.GetChat(chatUid, (int)pageNumber, (int)pageSize);
		}

		[HttpGet]
		[Route("get-person-chat")]
		public async Task<ActionResult<ChatModel>> GetPersonChat(Guid personUid)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _chatValidation.ValidateGetPersonChat(personUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			return await _chatLogic.GetPersonChat(uid, personUid);
		}

		[HttpGet]
		[Route("get-person-chat-list")]
		public async Task<ActionResult<List<ChatListModel>>> GetPersonChatList()
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			return await _chatLogic.GetPersonChatList(uid);
		}

		[HttpPost]
		[Route("add-chat-mesage")]
		public async Task<ActionResult<ChatMessageModel>> AddChatMessage(AddMessageModel request)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _chatValidation.ValidateAddChatMessage(request, uid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			return await _chatLogic.AddChatMessage(request, uid);
		}
	}
}
