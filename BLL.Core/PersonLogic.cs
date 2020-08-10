using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.Person;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using DAL.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class PersonLogic : IPersonLogic
	{
		private readonly IPersonRepository _personRepository;
		private readonly IEventRepository _eventRepository;
		private readonly IImageLogic _imageLogic;
		private readonly IMapper _mapper;

		private const int countOfFriends = 5;

		public PersonLogic(IPersonRepository personRepository,
			IEventRepository eventRepository,
			IImageLogic imageLogic,
			IMapper mapper)
		{
			_personRepository = personRepository;
			_eventRepository = eventRepository;
			_imageLogic = imageLogic;
			_mapper = mapper;
		}

		public async Task CreatePerson(Guid personUid)
		{
			var personExists = await _personRepository.CheckPersonExistence(personUid);
			if (!personExists)
			{
				await _personRepository.CreatePerson(personUid);
			}
		}

		public async Task<PersonModel> GetPerson(Guid personUid)
		{
			var entity = await _personRepository.GetPerson(personUid);
			var model = _mapper.Map<PersonModel>(entity);
			model.Friends = entity.FriendList.Select(x => _mapper.Map<PersonModel>(x.Friend)).Take(countOfFriends).ToList();
			foreach (var friend in model.Friends)
			{
				friend.IsFriend = true;
			}
			return model;
		}

		public async Task UpdatePerson(UpdatePersonModel updatePersonModel, Guid personUid)
		{
			var entity = await _personRepository.GetPerson(personUid);
			if (!string.IsNullOrEmpty(updatePersonModel.Name))
				entity.Name = updatePersonModel.Name;
			if (!string.IsNullOrEmpty(updatePersonModel.Description))
				entity.Description = updatePersonModel.Description;
			if (updatePersonModel.Age.HasValue)
				entity.Age = updatePersonModel.Age;
			if (updatePersonModel.CityId.HasValue)
				entity.CityId = updatePersonModel.CityId;
			if (!string.IsNullOrEmpty(updatePersonModel.Login))
				entity.Login = updatePersonModel.Login;
			if (!string.IsNullOrEmpty(updatePersonModel.Token))
				entity.Token = updatePersonModel.Token;
			if (updatePersonModel.Image != null)
			{
				if (entity.PersonImageContentEntity != null)
				{
					await _personRepository.RemovePersonImage(entity.PersonImageContentEntity);
					await _imageLogic.RemoveImage(entity.PersonImageContentEntity.PersonImageContentUid);
				}
				var imageUid = await _imageLogic.SaveImage(updatePersonModel.Image);
				entity.PersonImageContentEntity = new PersonImageContentEntity { PersonImageContentUid = imageUid };
			}
			entity.PersonImageContentEntity = null;
			entity.FriendList = null;
			entity.City = null;
			entity.SwipeHistory = null;
			await _personRepository.UpdatePerson(entity);
		}

		public async Task<bool> IsPersonFilledUp(Guid personUid)
		{
			var entity = await _personRepository.GetPerson(personUid);
			return entity != null &&
				!string.IsNullOrEmpty(entity.Name) &&
				entity.Age.HasValue &&
				!string.IsNullOrEmpty(entity.Login) &&
				entity.PersonImageContentEntity != null;
		}

		public async Task AddFriendToPerson(Guid personUid, Guid friendUid)
		{
			await _personRepository.AddFriendToPerson(personUid, friendUid);
		}

		public async Task RemoveFriendFromPerson(Guid personUid, Guid friendUid)
		{
			await _personRepository.RemoveFriendFromPerson(personUid, friendUid);
		}

		public async Task<IEnumerable<PersonModel>> GetPersonListByPage(Guid personUid, GetPersonListFilter model)
		{
			var filter = _mapper.Map<RepositoryGetPersonListFilter>(model);
			var persons = await _personRepository.GetPersonListByPage(personUid, filter);
			var personModels = _mapper.Map<IEnumerable<PersonModel>>(persons);
			foreach (var personModel in personModels)
			{
				personModel.IsFriend = await _personRepository.CheckPersonFriendExistence(personUid, personModel.PersonUid);
			}
			return personModels;
		}

		public async Task<bool> CheckFriendship(Guid personUid, Guid friendUid)
		{
			return await _personRepository.CheckPersonFriendExistence(personUid, friendUid);
		}

		public async Task<List<PersonModel>> GetAllPersonFriends(Guid personUid)
		{
			var entities = await _personRepository.GetAllPersonFriends(personUid);
			var models = _mapper.Map<List<PersonModel>>(entities);
			foreach (var model in models)
			{
				model.IsFriend = true;
			}
			return models;
		}

		public async Task<PersonModel> GetRandomPerson(RandomPersonFilter randomPersonFilter, Guid uid)
		{
			var personEntity = await _personRepository.GetPerson(uid);
			var eventEntity = await _eventRepository.GetEvent(randomPersonFilter.EventUid);
			var filter = _mapper.Map<RepositoryRandomPersonFilter>(randomPersonFilter);
			filter.EventId = eventEntity.EventId;
			filter.IgnoringPersonList = eventEntity.SwipeHistory.Select(x => x.PersonId).ToList();
			var randomPersonEntity = await _personRepository.GetRandomPerson(filter, personEntity.PersonId);
			return _mapper.Map<PersonModel>(randomPersonEntity);
		}

		public async Task AddPersonSwipeHistory(Guid eventUid, Guid personUid)
		{
			var eventEntity = await _eventRepository.GetEvent(eventUid);
			var personEntity = await _personRepository.GetPerson(personUid);
			await _personRepository.AddPersonSwipeHistoryRecord(new PersonSwipeHistoryEntity { PersonId = personEntity.PersonId, EventId = eventEntity.EventId });
		}
	}
}