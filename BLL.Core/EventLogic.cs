using BLL.Core.Interfaces;
using BLL.Core.Models.Event;
using Constants;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class EventLogic : IEventLogic
	{
		private readonly IEventRepository _eventRepository;
		public EventLogic(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
		}

		public async Task<Guid> AddEvent(AddEventModel addEventModel, Guid personUid)
		{
			var eventUid = Guid.NewGuid();
			var person = await _eventRepository.GetPerson(personUid);
			await _eventRepository.CreateEvent(new EventEntity
			{
				EventUid = eventUid,
				Name = addEventModel.Name,
				MinAge = addEventModel.MinAge,
				MaxAge = addEventModel.MaxAge,
				XCoordinate = addEventModel.XCoordinate,
				YCoordinate = addEventModel.YCoordinate,
				Description = addEventModel.Description,
				StartTime = addEventModel.StartTime,
				EndTime = addEventModel.EndTime,
				EventTypeId = (long)addEventModel.Type,
				EventStatusId = (long)addEventModel.Status,
				AdministratorId = person.PersonId
			});
			return eventUid;
		}

		public async Task<GetEventModel> GetEvent(Guid eventUid)
		{
			var eventEntity = await _eventRepository.GetEvent(eventUid);
			GetEventModel eventModel = EventEntityToModel(eventEntity);
			return eventModel;
		}

		public async Task<List<GetEventListModel>> GetEventList(Guid personUid)
		{
			var eventEntities = await _eventRepository.GetEvents(personUid);
			return eventEntities.Select(x => EventEntityToListModel(x, personUid)).ToList();
		}

		public async Task UpdateEvent(UpdateEventModel updateEventModel)
		{
			var eventEntity = await _eventRepository.GetEvent(updateEventModel.EventUid);
			if (!string.IsNullOrEmpty(updateEventModel.Name))
				eventEntity.Name = updateEventModel.Name;
			if (updateEventModel.MinAge.HasValue)
				eventEntity.MinAge = updateEventModel.MinAge;
			if (updateEventModel.MaxAge.HasValue)
				eventEntity.MaxAge = updateEventModel.MaxAge;
			if (updateEventModel.XCoordinate.HasValue)
				eventEntity.XCoordinate = updateEventModel.XCoordinate.Value;
			if (updateEventModel.YCoordinate.HasValue)
				eventEntity.YCoordinate = updateEventModel.YCoordinate.Value;
			if (!string.IsNullOrEmpty(updateEventModel.Description))
				eventEntity.Description = updateEventModel.Description;
			if (updateEventModel.StartTime.HasValue)
				eventEntity.StartTime = updateEventModel.StartTime;
			if (updateEventModel.EndTime.HasValue)
				eventEntity.EndTime = updateEventModel.EndTime;
			if (updateEventModel.Type.HasValue)
			{
				eventEntity.EventTypeId = (long)updateEventModel.Type;
				eventEntity.EventType = null;
			}
			if (updateEventModel.Status.HasValue)
			{
				eventEntity.EventStatusId = (long)updateEventModel.Status;
				eventEntity.EventStatus = null;
			}
			await _eventRepository.UpdateEvent(eventEntity);
		}


		private GetEventModel EventEntityToModel(EventEntity eventEntity)
		{
			return new GetEventModel
			{
				EventUid = eventEntity.EventUid,
				Name = eventEntity.Name,
				MinAge = eventEntity.MinAge,
				MaxAge = eventEntity.MaxAge,
				XCoordinate = eventEntity.XCoordinate,
				YCoordinate = eventEntity.YCoordinate,
				Description = eventEntity.Description,
				StartTime = eventEntity.StartTime.Value,
				EndTime = eventEntity.EndTime.Value,
				Type = (EventType)eventEntity.EventTypeId,
				Status = (EventStatus)eventEntity.EventStatusId,
				EventImageContentUid = eventEntity.EventImageContent?.EventImageContentUid,
				Administrator = PersonEntityToModel(eventEntity.Administrator),
				Participants = eventEntity.Participants.Select(x => PersonEntityToModel(x.Person)).ToList()
			};
		}

		private GetEventListModel EventEntityToListModel(EventEntity eventEntity, Guid personUid)
		{
			return new GetEventListModel
			{
				EventUid = eventEntity.EventUid,
				Name = eventEntity.Name,
				XCoordinate = eventEntity.XCoordinate,
				YCoordinate = eventEntity.YCoordinate,
				Description = eventEntity.Description,
				StartTime = eventEntity.StartTime.Value,
				EndTime = eventEntity.EndTime.Value,
				Type = (EventType)eventEntity.EventTypeId,
				Status = (EventStatus)eventEntity.EventStatusId,
				EventImageContentUid = eventEntity.EventImageContent?.EventImageContentUid,
				IsAdministrator = eventEntity.Administrator.PersonUid == personUid,

			};
		}
	}
}
