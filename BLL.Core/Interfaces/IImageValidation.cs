using System;

namespace BLL.Core.Interfaces
{
	public interface IImageValidation
	{
		(bool ValidationResult, string ValidationMessage) ValidateGetImage(Guid uid);
		(bool ValidationResult, string ValidationMessage) ValidateGetMiniatureImage(Guid imageUid);
	}
}
