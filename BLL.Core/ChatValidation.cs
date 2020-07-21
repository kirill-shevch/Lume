using BLL.Core.Interfaces;
using Constants;
using DAL.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace BLL.Core
{
	public class ChatValidation : IChatValidation
	{
		private readonly IPersonRepository _personRepository;
		private readonly IChatRepository _chatRepository;
		public ChatValidation(IPersonRepository personRepository,
			IChatRepository chatRepository)
		{
			_personRepository = personRepository;
			_chatRepository = chatRepository;
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

		public (bool ValidationResult, string ValidationMessage) ValidateGetChat(Guid chatUid)
		{
			if (!_chatRepository.CheckChatExistence(chatUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(17));
			}
			return (true, string.Empty);
		}
	}
}
