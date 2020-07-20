using BLL.Core.Models;
using BLL.Core.Models.Event;
using BLL.Core.Models.Person;
using System;

namespace BLL.Core.Interfaces
{
	public interface ICoreValidation
	{
		(bool ValidationResult, string ValidationMessage) ValidateUpdatePerson(UpdatePersonModel model);
		(bool ValidationResult, string ValidationMessage) ValidateAddEvent(AddEventModel model);
		(bool ValidationResult, string ValidationMessage) ValidateGetEvent(Guid eventUid);
		(bool ValidationResult, string ValidationMessage) ValidateUpdateEvent(UpdateEventModel model);
	}
}