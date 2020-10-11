using BLL.Core.Interfaces;
using BLL.Core.Models.Event;
using Constants;
using DAL.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace BLL.Core
{
	public class EventValidation : BaseValidator, IEventValidation
	{
		private readonly IEventRepository _eventRepository;
		private readonly IPersonRepository _personRepository;
		private readonly ICityLogic _cityLogic;
		public EventValidation(IEventRepository eventRepository,
			IPersonRepository personRepository,
			ICityLogic cityLogic,
			IHttpContextAccessor contextAccessor) : base(contextAccessor)
		{
			_eventRepository = eventRepository;
			_personRepository = personRepository;
			_cityLogic = cityLogic;
		}

		public (bool ValidationResult, string ValidationMessage) ValidateAddEvent(AddEventModel model)
		{
			if (string.IsNullOrWhiteSpace(model.Name))
			{
				return (false, ErrorDictionary.GetErrorMessage(16, _culture));
			}
			if (!Enum.IsDefined(typeof(EventStatus), model.Status))
			{
				return (false, ErrorDictionary.GetErrorMessage(13, _culture));
			}
			if (model.Types == null || !model.Types.Any())
			{
				return (false, ErrorDictionary.GetErrorMessage(36, _culture));
			}
			else
			{
				if (model.Types.Count != model.Types.Distinct().Count())
				{
					return (false, ErrorDictionary.GetErrorMessage(37, _culture));
				}
				if (model.Types.Count > 3)
				{
					return (false, ErrorDictionary.GetErrorMessage(35, _culture));
				}
				foreach (var type in model.Types)
				{
					if (!Enum.IsDefined(typeof(EventType), type))
					{
						return (false, ErrorDictionary.GetErrorMessage(14, _culture));
					}
				}
			}
			if (model.StartTime > model.EndTime)
			{
				return (false, ErrorDictionary.GetErrorMessage(42, _culture));
			}
			if (model.MinAge.HasValue && model.MaxAge.HasValue && model.MinAge > model.MaxAge)
			{
				return (false, ErrorDictionary.GetErrorMessage(15, _culture));
			}
			if (model.IsOnline.HasValue && !model.IsOnline.Value && !model.CityId.HasValue)
			{
				return (false, ErrorDictionary.GetErrorMessage(31, _culture));
			}
			if (model.IsOnline.HasValue && model.IsOnline.Value && model.CityId.HasValue)
			{
				return (false, ErrorDictionary.GetErrorMessage(32, _culture));
			}
			if (model.CityId.HasValue)
			{
				var cities = _cityLogic.GetCities().Result;
				if (!cities.Any(x => x.CityId == model.CityId))
				{
					return (false, ErrorDictionary.GetErrorMessage(30, _culture));
				}
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetEvent(Guid eventUid)
		{
			if (!_eventRepository.CheckEventExistence(eventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10, _culture));
			}

			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateGetRandomEvent(RandomEventFilter filter)
		{
			if (filter.PersonXCoordinate < 0)
			{
				return (false, ErrorDictionary.GetErrorMessage(23, _culture));
			}
			if (filter.PersonYCoordinate < 0)
			{
				return (false, ErrorDictionary.GetErrorMessage(23, _culture));
			}
			if (filter.CityId.HasValue)
			{
				var cities = _cityLogic.GetCities().Result;
				if (!cities.Any(x => x.CityId == filter.CityId))
				{
					return (false, ErrorDictionary.GetErrorMessage(30, _culture));
				}
			}
			if (filter.EventTypes != null && filter.EventTypes.Any())
			{
				foreach (var type in filter.EventTypes)
				{
					if (!Enum.IsDefined(typeof(EventType), type))
					{
						return (false, ErrorDictionary.GetErrorMessage(14, _culture));
					}
				}
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateUpdateParticipantModel(EventParticipantModel model)
		{
			if (!_personRepository.CheckPersonExistence(model.PersonUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2, _culture));
			}
			if (!_eventRepository.CheckEventExistence(model.EventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10, _culture));
			}
			var participant = _eventRepository.GetParticipant(model.PersonUid, model.EventUid).Result;
			if (participant == null)
			{
				return (false, ErrorDictionary.GetErrorMessage(26, _culture));
			}
			if (!Enum.IsDefined(typeof(ParticipantStatus), model.ParticipantStatus))
			{
				return (false, ErrorDictionary.GetErrorMessage(21, _culture));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateRemoveEventParticipant(Guid personUid, Guid eventUid)
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

		public (bool ValidationResult, string ValidationMessage) ValidateSearchForEvent(EventSearchFilter eventSearchFilter)
		{
			if (eventSearchFilter.MinAge.HasValue && eventSearchFilter.MaxAge.HasValue && eventSearchFilter.MinAge > eventSearchFilter.MaxAge)
			{
				return (false, ErrorDictionary.GetErrorMessage(15, _culture));
			}
			if (eventSearchFilter.Status.HasValue && !Enum.IsDefined(typeof(EventStatus), eventSearchFilter.Status))
			{
				return (false, ErrorDictionary.GetErrorMessage(13, _culture));
			}
			if (eventSearchFilter.Type.HasValue && !Enum.IsDefined(typeof(EventType), eventSearchFilter.Type))
			{
				return (false, ErrorDictionary.GetErrorMessage(14, _culture));
			}
			var cities = _cityLogic.GetCities().Result;
			if (eventSearchFilter.CityId.HasValue)
			{
				if (!cities.Any(x => x.CityId == eventSearchFilter.CityId))
				{
					return (false, ErrorDictionary.GetErrorMessage(30, _culture));
				}
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateUpdateEvent(UpdateEventModel model)
		{
			if (!_eventRepository.CheckEventExistence(model.EventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10, _culture));
			}
			if (model.Status.HasValue && !Enum.IsDefined(typeof(EventStatus), model.Status))
			{
				return (false, ErrorDictionary.GetErrorMessage(13, _culture));
			}
			if (model.Types != null)
			{
				if (model.Types.Count != model.Types.Distinct().Count())
				{
					return (false, ErrorDictionary.GetErrorMessage(37, _culture));
				}
				if (model.Types.Count > 3)
				{
					return (false, ErrorDictionary.GetErrorMessage(35, _culture));
				}
				foreach (var type in model.Types)
				{
					if (!Enum.IsDefined(typeof(EventType), type))
					{
						return (false, ErrorDictionary.GetErrorMessage(14, _culture));
					}
				}
			}
			if (model.MinAge.HasValue && model.MaxAge.HasValue && model.MinAge > model.MaxAge)
			{
				return (false, ErrorDictionary.GetErrorMessage(15, _culture));
			}
			if (model.StartTime.HasValue && model.EndTime.HasValue && model.StartTime > model.EndTime)
			{
				return (false, ErrorDictionary.GetErrorMessage(42, _culture));
			}
			if (model.CityId.HasValue)
			{
				var cities = _cityLogic.GetCities().Result;
				if (!cities.Any(x => x.CityId == model.CityId))
				{
					return (false, ErrorDictionary.GetErrorMessage(30, _culture));
				}
			}
			if (model.PrimaryImage != null && model.PrimaryImage.Any())
			{
				var eventModel = _eventRepository.GetEvent(model.EventUid).Result;
				if (eventModel.EventImageContentEntities.Any(x => x.IsPrimary.HasValue && x.IsPrimary.Value))
				{
					return (false, ErrorDictionary.GetErrorMessage(46, _culture));
				}
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateAddParticipantModel(EventParticipantModel model)
		{
			if (!_personRepository.CheckPersonExistence(model.PersonUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(2, _culture));
			}
			if (!_eventRepository.CheckEventExistence(model.EventUid).Result)
			{
				return (false, ErrorDictionary.GetErrorMessage(10, _culture));
			}
			var participant = _eventRepository.GetParticipant(model.PersonUid, model.EventUid).Result;
			if (participant != null)
			{
				return (false, ErrorDictionary.GetErrorMessage(24, _culture));
			}
			if (!Enum.IsDefined(typeof(ParticipantStatus), model.ParticipantStatus))
			{
				return (false, ErrorDictionary.GetErrorMessage(21, _culture));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateAddPromoRewardRequest(PromoRewardRequestModel request)
		{
			if (request.Images == null || request.Images.Count < 1 || request.Images.Count > 2)
			{
				return (false, ErrorDictionary.GetErrorMessage(47, _culture));
			}
			if (string.IsNullOrWhiteSpace(request.AccountingNumber))
			{
				return (false, ErrorDictionary.GetErrorMessage(48, _culture));
			}
			var eventEntity = _eventRepository.GetEvent(request.EventUid).Result;
			if (eventEntity == null)
			{
				return (false, ErrorDictionary.GetErrorMessage(10, _culture));
			}
			if (eventEntity.EventStatusId != (long)EventStatus.Ended)
			{
				return (false, ErrorDictionary.GetErrorMessage(49, _culture));
			}
			return (true, string.Empty);
		}

		public (bool ValidationResult, string ValidationMessage) ValidateRemoveEventImage(Guid personUid, RemoveEventImageModel request)
		{
			var eventEntity = _eventRepository.GetEvent(request.EventUid).Result;
			if (eventEntity == null)
			{
				return (false, ErrorDictionary.GetErrorMessage(10, _culture));
			}
			if (eventEntity.Administrator.PersonUid != personUid)
			{
				return (false, ErrorDictionary.GetErrorMessage(55, _culture));
			}
			if (!eventEntity.EventImageContentEntities.Any(x => x.EventImageContentUid == request.ImageUid))
			{
				return (false, ErrorDictionary.GetErrorMessage(56, _culture));
			}
			return (true, string.Empty);
		}
	}
}
