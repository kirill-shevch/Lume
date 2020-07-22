using BLL.Core.Models.Chat;
using System;
using System.Collections.Generic;

namespace BLL.Core.Interfaces
{
	public interface IChatValidation
	{
		(bool ValidationResult, string ValidationMessage) ValidateAddChat(List<Guid> chatParticipants);
		(bool ValidationResult, string ValidationMessage) ValidateGetChat(Guid chatUid);
		(bool ValidationResult, string ValidationMessage) ValidateGetPersonChat(Guid personUid);
		(bool ValidationResult, string ValidationMessage) ValidateAddChatMessage(AddMessageModel request, Guid personUid);
		(bool ValidationResult, string ValidationMessage) ValidateGetNewChatMessages(Guid chatUid, Guid messageUid);
	}
}
