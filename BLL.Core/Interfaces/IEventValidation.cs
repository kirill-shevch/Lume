using BLL.Core.Models.Event;
using System;

namespace BLL.Core.Interfaces
{
	public interface IEventValidation
	{
		(bool ValidationResult, string ValidationMessage) ValidateAddEvent(AddEventModel model);
		(bool ValidationResult, string ValidationMessage) ValidateGetEvent(Guid eventUid);
		(bool ValidationResult, string ValidationMessage) ValidateUpdateEvent(UpdateEventModel model);
	}
}
