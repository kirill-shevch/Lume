using BLL.Core.Interfaces;
using BLL.Core.Models.Chat;
using Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

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
		public async Task<ActionResult<ChatModel>> GetChat(Guid chatUid, int pageNumber, int pageSize) 
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _chatValidation.ValidateGetChat(chatUid, pageNumber, pageSize, uid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			return await _chatLogic.GetChat(chatUid, pageNumber, pageSize, uid);
		}

		[HttpGet]
		[Route("get-person-chat")]
		public async Task<ActionResult<ChatModel>> GetPersonChat(Guid personUid, uint pageSize)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _chatValidation.ValidateGetPersonChat(uid, personUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			return await _chatLogic.GetPersonChat(uid, personUid, (int)pageSize);
		}

		[HttpGet]
		[Route("get-person-chat-list")]
		public async Task<ActionResult<List<ChatListModel>>> GetPersonChatList()
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			return await _chatLogic.GetPersonChatList(uid);
		}

		[HttpPost]
		[Route("add-chat-message")]
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

		[HttpGet]
		[Route("get-new-chat-messages")]
		public async Task<ActionResult<List<ChatMessageModel>>> GetNewChatMessages(Guid chatUid, Guid? messageUid)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _chatValidation.ValidateGetNewChatMessages(chatUid, messageUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			return await _chatLogic.GetNewChatMessages(chatUid, messageUid, uid);
		}

		[HttpPost]
		[Route("mute-chat")]
		public async Task<ActionResult> MuteChat(Guid chatUid, bool mute)
		{
			var uid = new Guid(HttpContext.Request.Headers[AuthorizationHeaders.PersonUid].First());
			var validationResult = _chatValidation.ValidateMuteChat(chatUid, uid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			await _chatLogic.MuteChat(chatUid, mute, uid);
			return Ok(Messages.GetMessageJson(MessageTitles.OperationsSuccessful, CultureParser.GetCultureFromHttpContext(HttpContext)));
		}
	} 
}
