﻿using BLL.Core.Interfaces;
using BLL.Core.Models.Person;
using Constants;
using DAL.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace BLL.Core
{
	public class PersonValidation : BaseValidator, IPersonValidation
	{
		private readonly IPersonRepository _personRepository;
		private readonly IEventRepository _eventRepository;
		private readonly ICityLogic _cityLogic;
		
		public PersonValidation(IPersonRepository personRepository,
			IEventRepository eventRepository,
			ICityLogic cityLogic,
			IHttpContextAccessor contextAccessor) : base(contextAccessor)
		{
			_personRepository = personRepository;
			_eventRepository = eventRepository;
			_cityLogic = cityLogic;
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetPerson(Guid personUid)
		{
			if (!_personRepository.CheckPersonExistence(personUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2, _culture));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetPersonListByPage(GetPersonListFilter request)
		{
			if (request.PageNumber < 1)
			{
				return (false, ErrorDictionary.GetErrorMessage(28, _culture));
			}
			if (request.PageSize < 1)
			{
				return (false, ErrorDictionary.GetErrorMessage(29, _culture));
			}
			if (request.CityId.HasValue)
			{
				var cities = _cityLogic.GetCities().Result;
				if (!cities.Any(x => x.CityId == request.CityId.Value))
				{
					return (false, ErrorDictionary.GetErrorMessage(30, _culture));
				}
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetRandomPerson(RandomPersonFilter randomPersonFilter)
		{
			if (randomPersonFilter.MinAge.HasValue && (randomPersonFilter.MinAge < 0 || randomPersonFilter.MinAge > 150))
			{
				return (false, ErrorDictionary.GetErrorMessage(22, _culture));
			}
			if (randomPersonFilter.MaxAge.HasValue && (randomPersonFilter.MaxAge < 0 || randomPersonFilter.MaxAge > 150))
			{
				return (false, ErrorDictionary.GetErrorMessage(22, _culture));
			}
			if (randomPersonFilter.MinAge.HasValue && randomPersonFilter.MaxAge.HasValue && randomPersonFilter.MinAge > randomPersonFilter.MaxAge)
			{
				return (false, ErrorDictionary.GetErrorMessage(15, _culture));
			}
			if (!_eventRepository.CheckEventExistence(randomPersonFilter.EventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10, _culture));
			}
			if (randomPersonFilter.CityId.HasValue)
			{
				var cities = _cityLogic.GetCities().Result;
				if (!cities.Any(x => x.CityId == randomPersonFilter.CityId.Value))
				{
					return (false, ErrorDictionary.GetErrorMessage(30, _culture));
				}
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateRejectRandomPerson(Guid eventUid, Guid personUid)
		{
			if (!_personRepository.CheckPersonExistence(personUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2, _culture));
			}
			if (!_eventRepository.CheckEventExistence(eventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10, _culture));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateUpdatePerson(UpdatePersonModel model)
		{
			if (!_personRepository.CheckPersonExistence(model.PersonUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2, _culture));
			}
			if (!string.IsNullOrEmpty(model.Login) && _personRepository.CheckPersonExistence(model.Login).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(33, _culture));
			}
			if (model.Age.HasValue && model.Age.Value < 0)
			{
				return (false, ErrorDictionary.GetErrorMessage(22, _culture));
			}
			if (model.CityId.HasValue)
			{
				var cities = _cityLogic.GetCities().Result;
				if (!cities.Any(x => x.CityId == model.CityId.Value))
				{
					return (false, ErrorDictionary.GetErrorMessage(30, _culture));
				}
			}
			return (true, string.Empty);
		}
	}
}
