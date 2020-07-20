using BLL.Core.Interfaces;
using BLL.Core.Models.Event;
using BLL.Core.Models.Person;
using Constants;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class CoreLogic : ICoreLogic
	{
		private readonly ICoreRepository _coreRepository;
		public CoreLogic(ICoreRepository coreRepository)
		{
			_coreRepository = coreRepository;
		}

		#region person
		public async Task CreatePerson(Guid personUid)
		{
			var personExists = await _coreRepository.CheckPersonExistence(personUid);
			if (!personExists)
			{
				await _coreRepository.CreatePerson(personUid);
			}
		}

		public async Task<PersonModel> GetPerson(Guid personUid)
		{
			var entity = await _coreRepository.GetPerson(personUid);
			var model = PersonEntityToModel(entity);
			model.Friends = entity.FriendList.Select(x => PersonEntityToModel(x.Friend)).ToList();
			return model;
		}

		public async Task UpdatePerson(UpdatePersonModel updatePersonModel)
		{
			var entity = await _coreRepository.GetPerson(updatePersonModel.PersonUid);
			if (!string.IsNullOrEmpty(updatePersonModel.Name)) 
				entity.Name = updatePersonModel.Name;
			if (!string.IsNullOrEmpty(updatePersonModel.Description))
				entity.Description = updatePersonModel.Description;
			if (updatePersonModel.Age.HasValue)
				entity.Age = updatePersonModel.Age;
			await _coreRepository.UpdatePerson(entity);
		}

		public async Task<bool> IsPersonFilledUp(Guid personUid)
		{
			var entity = await _coreRepository.GetPerson(personUid);
			return entity != null && 
				!string.IsNullOrEmpty(entity.Name) && 
				entity.Age.HasValue;
		}
		#endregion person

		#region event
		public async Task<Guid> AddEvent(AddEventModel addEventModel, Guid personUid)
		{
			var eventUid = Guid.NewGuid();
			var person = await _coreRepository.GetPerson(personUid);
			await _coreRepository.CreateEvent(new EventEntity 
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
			var eventEntity = await _coreRepository.GetEvent(eventUid);
			GetEventModel eventModel = EventEntityToModel(eventEntity);
			return eventModel;
		}

		public async Task<List<GetEventListModel>> GetEventList(Guid personUid)
		{
			var eventEntities = await _coreRepository.GetEvents(personUid);
			return eventEntities.Select(x => EventEntityToListModel(x, personUid)).ToList();
		}

		public async Task UpdateEvent(UpdateEventModel updateEventModel)
		{
			var eventEntity = await _coreRepository.GetEvent(updateEventModel.EventUid);
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
			await _coreRepository.UpdateEvent(eventEntity);
		}
		#endregion event

		private PersonModel PersonEntityToModel(PersonEntity entity)
		{
			return new PersonModel
			{
				PersonUid = entity.PersonUid,
				Name = entity.Name,
				Age = entity.Age,
				Description = entity.Description,
				ImageContentUid = entity.PersonImageContentEntity?.PersonImageContentUid
			};
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