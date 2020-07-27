using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.Person;
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
		private readonly IMapper _mapper;

		private const int countOfFriends = 5;

		public PersonLogic(IPersonRepository personRepository,
			IEventRepository eventRepository,
			IMapper mapper)
		{
			_personRepository = personRepository;
			_eventRepository = eventRepository;
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

		public async Task UpdatePerson(UpdatePersonModel updatePersonModel)
		{
			var entity = await _personRepository.GetPerson(updatePersonModel.PersonUid);
			if (!string.IsNullOrEmpty(updatePersonModel.Name))
				entity.Name = updatePersonModel.Name;
			if (!string.IsNullOrEmpty(updatePersonModel.Description))
				entity.Description = updatePersonModel.Description;
			if (updatePersonModel.Age.HasValue)
				entity.Age = updatePersonModel.Age;
			await _personRepository.UpdatePerson(entity);
		}

		public async Task<bool> IsPersonFilledUp(Guid personUid)
		{
			var entity = await _personRepository.GetPerson(personUid);
			return entity != null &&
				!string.IsNullOrEmpty(entity.Name) &&
				entity.Age.HasValue;
		}

		public async Task AddFriendToPerson(Guid personUid, Guid friendUid)
		{
			await _personRepository.AddFriendToPerson(personUid, friendUid);
		}

		public async Task RemoveFriendFromPerson(Guid personUid, Guid friendUid)
		{
			await _personRepository.RemoveFriendFromPerson(personUid, friendUid);
		}

		public async Task<IEnumerable<PersonModel>> GetPersonListByPage(GetPersonListModel model)
		{
			var persons = await _personRepository.GetPersonListByPage(model.PageNumber, model.PageSize, model.Query);
			var personModels = _mapper.Map<IEnumerable<PersonModel>>(persons);
			foreach (var personModel in personModels)
			{
				personModel.IsFriend = await _personRepository.CheckPersonFriendExistence(model.PersonUid, personModel.PersonUid);
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
			var randomPersonEntity = await _personRepository.GetRandomPerson(filter, personEntity.PersonId);
			return _mapper.Map<PersonModel>(randomPersonEntity);
		}
	}
}