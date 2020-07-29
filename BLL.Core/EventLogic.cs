using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.Event;
using Constants;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using DAL.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
			var imageList = new List<EventImageContentEntity>();
			if (addEventModel.PrimaryImage != null && addEventModel.PrimaryImage.Any())
			{
				imageList.Add(new EventImageContentEntity 
				{ 
					Content = addEventModel.PrimaryImage, 
					EventImageContentUid = Guid.NewGuid(), 
					IsPrimary = true 
				});
			}
			if (addEventModel.Images != null)
			{
				foreach (var image in addEventModel.Images)
				{
					imageList.Add(new EventImageContentEntity
					{
						Content = image,
						EventImageContentUid = Guid.NewGuid(),
						IsPrimary = false
					});
				}
			}
			entity.EventImageContentEntities = imageList;
			await _eventRepository.CreateEvent(entity);
			await AddParticipant(new EventParticipantModel { EventUid = eventUid, PersonUid = person.PersonUid, ParticipantStatus = ParticipantStatus.Active });
			return eventUid;
		}

		public async Task<GetEventModel> GetEvent(Guid eventUid)
		{
			var eventEntity = await _eventRepository.GetEvent(eventUid);
			var eventModel = _mapper.Map<GetEventModel>(eventEntity);
			foreach (var participant in eventModel.Participants)
			{
				var status = eventEntity.Participants.Single(x => x.Person.PersonUid == participant.PersonUid).ParticipantStatusId;
				participant.ParticipantStatus = (ParticipantStatus)status;
			}
			return eventModel;
		}

		public async Task<List<GetEventListModel>> GetEventList(Guid personUid)
		{
			var eventEntities = await _eventRepository.GetEvents(personUid);
			return eventEntities.Select(entity => 
			{
				var model = _mapper.Map<GetEventListModel>(entity);
				model.IsAdministrator = entity.Administrator.PersonUid == personUid;
				var status = entity.Participants.Single(s => s.Person.PersonUid == personUid).ParticipantStatusId;
				model.ParticipantStatus = (ParticipantStatus)status;
				model.AnyPersonWaitingForApprove = model.IsAdministrator.Value && entity.Participants.Any(x => x.ParticipantStatusId == (long)ParticipantStatus.WaitingForApproveFromEvent);
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
			if (updateEventModel.IsOpenForInvitations.HasValue)
				eventEntity.IsOpenForInvitations = updateEventModel.IsOpenForInvitations;
			if (updateEventModel.CityId.HasValue)
				eventEntity.CityId = updateEventModel.CityId;
			if (updateEventModel.Type.HasValue)
			{
				eventEntity.EventTypeId = (long)updateEventModel.Type;
			}
			if (updateEventModel.Status.HasValue)
			{
				eventEntity.EventStatusId = (long)updateEventModel.Status;
			}
			eventEntity.EventType = null;
			eventEntity.EventStatus = null;
			eventEntity.City = null;
			eventEntity.EventImageContentEntities = null;
			eventEntity.Administrator = null;
			eventEntity.Participants = null;
			eventEntity.Chat = null;
			eventEntity.SwipeHistory = null;
			await _eventRepository.UpdateEvent(eventEntity);
		}

		public async Task AddParticipant(EventParticipantModel eventParticipantModel)
		{
			var entity = await CreateParticipantEntity(eventParticipantModel);
			await _eventRepository.AddParticipant(entity);
		}

		public async Task UpdateParticipant(EventParticipantModel eventParticipantModel)
		{
			var entity = await CreateParticipantEntity(eventParticipantModel);
			await _eventRepository.UpdateParticipant(entity);
		}

		public async Task RemoveParticipant(Guid personUid, Guid eventUid)
		{
			var entity = await _eventRepository.GetParticipant(personUid, eventUid);
			await _eventRepository.RemoveParticipant(entity);
		}

		private async Task<PersonToEventEntity> CreateParticipantEntity(EventParticipantModel eventParticipantModel)
		{
			var personEntity = await _personRepository.GetPerson(eventParticipantModel.PersonUid);
			var eventEntity = await _eventRepository.GetEvent(eventParticipantModel.EventUid);
			return new PersonToEventEntity
			{
				EventId = eventEntity.EventId,
				PersonId = personEntity.PersonId,
				ParticipantStatusId = (long)eventParticipantModel.ParticipantStatus
			};
		}

		public async Task<GetEventModel> GetRandomEvent(RandomEventFilter filter, Guid personUid)
		{
			var repositoryFilter = _mapper.Map<RepositoryRandomEventFilter>(filter);
			var personEntity = await _personRepository.GetPerson(personUid);
			repositoryFilter.Age = personEntity.Age.Value;
			repositoryFilter.PersonUid = personUid;
			repositoryFilter.IgnoringEventList = personEntity.SwipeHistory.Select(x => x.EventId).ToList();
			var entity = await _eventRepository.GetRandomEvent(repositoryFilter);
			return _mapper.Map<GetEventModel>(entity);
		}

		public async Task<List<GetEventListModel>> SearchForEvent(EventSearchFilter filter)
		{
			var repositoryFilter = _mapper.Map<RepositoryEventSearchFilter>(filter);
			var entities = await _eventRepository.SearchForEvent(repositoryFilter);
			return _mapper.Map<List<GetEventListModel>>(entities);
		}

		public async Task AddEventSwipeHistory(Guid eventUid, Guid personUid)
		{
			var eventEntity = await _eventRepository.GetEvent(eventUid);
			var personEntity = await _personRepository.GetPerson(personUid);
			await _eventRepository.AddEventSwipeHistoryRecord(new EventSwipeHistoryEntity { EventId = eventEntity.EventId, PersonId = personEntity.PersonId });
		}
	}
}
