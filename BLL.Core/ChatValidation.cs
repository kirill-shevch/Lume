using BLL.Core.Interfaces;
using BLL.Core.Models.Chat;
using Constants;
using DAL.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Core
{
	public class ChatValidation : IChatValidation
	{
		private readonly IPersonRepository _personRepository;
		private readonly IChatRepository _chatRepository;
		private readonly IChatLogic _chatLogic;
		public ChatValidation(IPersonRepository personRepository,
			IChatRepository chatRepository,
			IChatLogic chatLogic)
		{
			_personRepository = personRepository;
			_chatRepository = chatRepository;
			_chatLogic = chatLogic;
		}

		public (bool ValidationResult, string ValidationMessage) ValidateAddChat(List<Guid> chatParticipants)
		{
			foreach (var personUid in chatParticipants)
			{
				if (!_personRepository.CheckPersonExistence(personUid).Result)
				{
					return (false, ErrorDictionary.GetErrorMessage(2));
				}
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateAddChatMessage(AddMessageModel request, Guid personUid)
		{
			var personChatList = _chatLogic.GetPersonChatList(personUid).Result;
			if (!personChatList.Any(x => x.ChatUid == request.ChatUid))
			{
				return (false, ErrorDictionary.GetErrorMessage(20));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetChat(Guid chatUid, int pageNumber, int pageSize)
		{
			if (!_chatRepository.CheckChatExistence(chatUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(19));
			}
			if (pageNumber < 1)
			{
				return (false, ErrorDictionary.GetErrorMessage(28));
			}
			if (pageSize < 1)
			{
				return (false, ErrorDictionary.GetErrorMessage(29));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetNewChatMessages(Guid chatUid, Guid? messageUid)
		{
			if (!_chatRepository.CheckChatExistence(chatUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(19));
			}
			if (messageUid.HasValue && !_chatRepository.CheckChatMessageExistence(messageUid.Value).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(11));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetPersonChat(Guid personUid)
		{
			if (!_personRepository.CheckPersonExistence(personUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2));
			}
			return (true, string.Empty);
		}
	}
}
