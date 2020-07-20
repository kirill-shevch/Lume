using BLL.Core.Interfaces;
using BLL.Core.Models;
using Constants;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using System;
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
			var eventModel = new GetEventModel
			{
				EventUid = eventUid,
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
			return eventModel;
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
	}
}