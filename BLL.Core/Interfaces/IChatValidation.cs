using System;
using System.Collections.Generic;

namespace BLL.Core.Interfaces
{
	public interface IChatValidation
	{
		(bool ValidationResult, string ValidationMessage) ValidateAddChat(List<Guid> chatParticipants);
		(bool ValidationResult, string ValidationMessage) ValidateGetChat(Guid chatUid);

	}
}
