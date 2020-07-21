using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.Event;
using BLL.Core.Models.Person;
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
		private readonly IPersonRepository _personRepository;
		private readonly IMapper _mapper;

		public EventLogic(IEventRepository eventRepository,
			IPersonRepository personRepository,
			IMapper mapper)
		{
			_eventRepository = eventRepository;
			_personRepository = personRepository;
			_mapper = mapper;
		}

		public async Task<Guid> AddEvent(AddEventModel addEventModel, Guid personUid)
		{
			var eventUid = Guid.NewGuid();
			var person = await _personRepository.GetPerson(personUid);
			var entity = _mapper.Map<EventEntity>(addEventModel);
			entity.EventUid = eventUid;
			entity.AdministratorId = person.PersonId;
			entity.Chat = new ChatEntity { ChatUid = Guid.NewGuid(), IsGroupChat = true };
			await _eventRepository.CreateEvent(entity);
			return eventUid;
		}

		public async Task<GetEventModel> GetEvent(Guid eventUid)
		{
			var eventEntity = await _eventRepository.GetEvent(eventUid);
			var eventModel = _mapper.Map<GetEventModel>(eventEntity);
			return eventModel;
		}

		public async Task<List<GetEventListModel>> GetEventList(Guid personUid)
		{
			var eventEntities = await _eventRepository.GetEvents(personUid);
			return eventEntities.Select(entity => 
			{
				var model = _mapper.Map<GetEventListModel>(entity);
				model.IsAdministrator = entity.Administrator.PersonUid == personUid;
				return model;
			}).ToList();
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
	}
}
