using BLL.Core.Models.Person;
using System;

namespace BLL.Core.Interfaces
{
	public interface IPersonValidation
	{
		(bool ValidationResult, string ValidationMessage) ValidateUpdatePerson(UpdatePersonModel model, Guid personUid);
		(bool ValidationResult, string ValidationMessage) ValidateGetPerson(Guid personUid);
		(bool ValidationResult, string ValidationMessage) ValidateGetRandomPerson(RandomPersonFilter randomPersonFilter);
		(bool ValidationResult, string ValidationMessage) ValidateGetPersonListByPage(GetPersonListFilter request);
		(bool ValidationResult, string ValidationMessage) ValidateRejectRandomPerson(Guid eventUid, Guid personUid);
		(bool ValidationResult, string ValidationMessage) ValidateFeedback(FeedbackModel model);
		(bool ValidationResult, string ValidationMessage) ValidateAddReport(PersonReportModel model);
	}
}
