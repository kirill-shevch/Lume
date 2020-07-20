using BLL.Core.Models.Person;

namespace BLL.Core.Interfaces
{
	public interface IPersonValidation
	{
		(bool ValidationResult, string ValidationMessage) ValidateUpdatePerson(UpdatePersonModel model);
	}
}
