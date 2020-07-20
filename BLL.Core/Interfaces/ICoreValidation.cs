using BLL.Core.Models;
using System;

namespace BLL.Core.Interfaces
{
	public interface ICoreValidation
	{
		(bool ValidationResult, string ValidationMessage) ValidateUpdatePerson(UpdatePersonModel model);
		(bool ValidationResult, string ValidationMessage) ValidateAddEvent(AddEventModel model);
		(bool ValidationResult, string ValidationMessage) ValidateGetEvent(Guid eventUid);

	}
}