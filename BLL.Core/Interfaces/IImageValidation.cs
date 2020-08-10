using BLL.Core.Models.Image;
using System;

namespace BLL.Core.Interfaces
{
	public interface IImageValidation
	{
		(bool ValidationResult, string ValidationMessage) ValidateGetImage(Guid uid);
	}
}
