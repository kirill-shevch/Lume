using BLL.Core.Interfaces;
using BLL.Core.Models.Event;
using Constants;
using DAL.Core.Interfaces;
using System;

namespace BLL.Core
{
	public class EventValidation : IEventValidation
	{
		private readonly IEventRepository _eventRepository;
		private readonly IPersonRepository _personRepository;
		public EventValidation(IEventRepository eventRepository,
			IPersonRepository personRepository)
		{
			_eventRepository = eventRepository;
			_personRepository = personRepository;
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
			if (!_eventRepository.CheckEventExistence(eventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetRandomEvent(RandomEventFilter filter)
		{
			if (filter.Age < 1 || filter.Age > 150)
			{
				return (false, ErrorDictionary.GetErrorMessage(22));
			}
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

		public (bool ValidationResult, string ValidationMessage) ValidateParticipantModel(EventParticipantModel model)
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
			return (true, string.Empty);
		}
	}
}
