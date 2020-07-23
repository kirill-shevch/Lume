using BLL.Core.Interfaces;
using BLL.Core.Models.Person;
using Constants;
using DAL.Core.Interfaces;
using System;

namespace BLL.Core
{
	public class PersonValidation : IPersonValidation
	{
		private readonly IPersonRepository _personRepository;
		public PersonValidation(IPersonRepository personRepository)
		{
			_personRepository = personRepository;
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetPerson(Guid personUid)
		{
			if (!_personRepository.CheckPersonExistence(personUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateUpdatePerson(UpdatePersonModel model)
		{
			if (!_personRepository.CheckPersonExistence(model.PersonUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2));
			}
			return (true, string.Empty);
		}
	}
}
