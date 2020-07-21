using BLL.Core.Interfaces;
using BLL.Core.Models.Chat;
using Microsoft.AspNetCore.Mvc;
using System;
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


		[HttpPost]
		[Route("get-chat")]
		public async Task<ActionResult<ChatModel>> GetGroupChat(Guid chatUid) 
		{
			var validationResult = _chatValidation.ValidateGetChat(chatUid);
			if (!validationResult.ValidationResult)
			{
				return BadRequest(validationResult.ValidationMessage);
			}
			return await _chatLogic.GetChat(chatUid);
		}
	}
}
