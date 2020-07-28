using BLL.Core.Interfaces;
using BLL.Core.Models.Event;
using Constants;
using DAL.Core.Interfaces;
using System;
using System.Linq;

namespace BLL.Core
{
	public class EventValidation : IEventValidation
	{
		private readonly IEventRepository _eventRepository;
		private readonly IPersonRepository _personRepository;
		private readonly ICityLogic _cityLogic;
		public EventValidation(IEventRepository eventRepository,
			IPersonRepository personRepository,
			ICityLogic cityLogic)
		{
			_eventRepository = eventRepository;
			_personRepository = personRepository;
			_cityLogic = cityLogic;
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
			var cities = _cityLogic.GetCities().Result;
			if (!cities.Any(x => x.CityId == model.CityId))
			{
				return (false, ErrorDictionary.GetErrorMessage(30));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetEvent(Guid eventUid)
		{
			if (!_eventRepository.CheckEventExistence(eventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetRandomEvent(RandomEventFilter filter)
		{
			if (filter.PersonXCoordinate < 0)
			{
				return (false, ErrorDictionary.GetErrorMessage(23));
			}
			if (filter.PersonYCoordinate < 0)
			{
				return (false, ErrorDictionary.GetErrorMessage(23));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateUpdateParticipantModel(EventParticipantModel model)
		{
			if (!_personRepository.CheckPersonExistence(model.PersonUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2));
			}
			if (!_eventRepository.CheckEventExistence(model.EventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10));
			}
			var participant = _eventRepository.GetParticipant(model.PersonUid, model.EventUid).Result;
			if (participant == null)
			{
				return (false, ErrorDictionary.GetErrorMessage(26));
			}
			if (!Enum.IsDefined(typeof(ParticipantStatus), model.ParticipantStatus))
			{
				return (false, ErrorDictionary.GetErrorMessage(21));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateRemoveEventParticipant(Guid personUid, Guid eventUid)
		{
			if (!_personRepository.CheckPersonExistence(personUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2));
			}
			if (!_eventRepository.CheckEventExistence(eventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateSearchForEvent(EventSearchFilter eventSearchFilter)
		{
			if (eventSearchFilter.MinAge.HasValue && eventSearchFilter.MaxAge.HasValue && eventSearchFilter.MinAge > eventSearchFilter.MaxAge)
			{
				return (false, ErrorDictionary.GetErrorMessage(15));
			}
			if (eventSearchFilter.Status.HasValue && !Enum.IsDefined(typeof(EventStatus), eventSearchFilter.Status))
			{
				return (false, ErrorDictionary.GetErrorMessage(13));
			}
			if (eventSearchFilter.Type.HasValue && !Enum.IsDefined(typeof(EventType), eventSearchFilter.Type))
			{
				return (false, ErrorDictionary.GetErrorMessage(14));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateUpdateEvent(UpdateEventModel model)
		{
			if (!_eventRepository.CheckEventExistence(model.EventUid).Result)
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
			var cities = _cityLogic.GetCities().Result;
			if (!cities.Any(x => x.CityId == model.CityId))
			{
				return (false, ErrorDictionary.GetErrorMessage(30));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateAddParticipantModel(EventParticipantModel model)
		{
			if (!_personRepository.CheckPersonExistence(model.PersonUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2));
			}
			if (!_eventRepository.CheckEventExistence(model.EventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10));
			}
			var participant = _eventRepository.GetParticipant(model.PersonUid, model.EventUid).Result;
			if (participant != null)
			{
				return (false, ErrorDictionary.GetErrorMessage(24));
			}
			if (!Enum.IsDefined(typeof(ParticipantStatus), model.ParticipantStatus))
			{
				return (false, ErrorDictionary.GetErrorMessage(21));
			}
			return (true, string.Empty);
		}
	}
}
