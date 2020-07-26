using BLL.Core.Models.Image;
using System;

namespace BLL.Core.Interfaces
{
	public interface IImageValidation
	{
		(bool ValidationResult, string ValidationMessage) ValidateAddPersonImage(AddPersonImageModel model, Guid personUid);
		(bool ValidationResult, string ValidationMessage) ValidateGetChatMessageImage(Guid uid);
		(bool ValidationResult, string ValidationMessage) ValidateGetEventImage(Guid uid);
		(bool ValidationResult, string ValidationMessage) ValidateGetPersonImage(Guid uid);
	}
}
