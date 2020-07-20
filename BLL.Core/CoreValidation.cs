using BLL.Core.Interfaces;
using BLL.Core.Models;
using Constants;
using DAL.Core.Interfaces;
using System;

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
			if (!_coreRepository.CheckPersonExistence(model.PersonUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateAddEvent(AddEventModel model)
		{
			if (string.IsNullOrEmpty(model.Name))
			{
				return (false, ErrorDictionary.GetErrorMessage(16));
			}
			if (!Enum.IsDefined(typeof(EventStatus), model.Status))
			{
				return (false, ErrorDictionary.GetErrorMessage(13));
			}
			if (!Enum.IsDefined(typeof(EventType), model.Type))
			{
				return (false, ErrorDictionary.GetErrorMessage(14));
			}
			if (model.MinAge.HasValue && model.MaxAge.HasValue && model.MinAge > model.MaxAge)
			{
				return (false, ErrorDictionary.GetErrorMessage(15));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetEvent(Guid eventUid)
		{
			if (!_coreRepository.CheckEventExistence(eventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateUpdateEvent(UpdateEventModel model)
		{
			if (!_coreRepository.CheckEventExistence(model.EventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10));
			}
			if (model.Status.HasValue && !Enum.IsDefined(typeof(EventStatus), model.Status))
			{
				return (false, ErrorDictionary.GetErrorMessage(13));
			}
			if (model.Type.HasValue && !Enum.IsDefined(typeof(EventType), model.Type))
			{
				return (false, ErrorDictionary.GetErrorMessage(14));
			}
			if (model.MinAge.HasValue && model.MaxAge.HasValue && model.MinAge > model.MaxAge)
			{
				return (false, ErrorDictionary.GetErrorMessage(15));
			}
			return (true, string.Empty);
		}
	}
}
