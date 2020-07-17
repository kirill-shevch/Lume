using BLL.Core.Interfaces;
using BLL.Core.Models;
using Constants;
using DAL.Core.Interfaces;

namespace BLL.Core
{
	public class CoreValidation : ICoreValidation
	{
		private readonly ICoreRepository _coreRepository;
		public CoreValidation(ICoreRepository coreRepository)
		{
			_coreRepository = coreRepository;
		}

		public (bool ValidationResult, string ValidationMessage) ValidateUpdatePerson(UpdatePersonModel model)
		{
			if (_coreRepository.CheckPersonExistence(model.PersonUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2));
			}

			return (true, string.Empty);
		}
	}
}
