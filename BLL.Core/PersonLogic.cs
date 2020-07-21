using AutoMapper;
using BLL.Core.Interfaces;
using BLL.Core.Models.Person;
using DAL.Core.Entities;
using DAL.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Core
{
	public class PersonLogic : IPersonLogic
	{
		private readonly IPersonRepository _personRepository;
		private readonly IMapper _mapper;
		public PersonLogic(IPersonRepository personRepository,
			IMapper mapper)
		{
			_personRepository = personRepository;
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
			model.Friends = entity.FriendList.Select(x => _mapper.Map<PersonModel>(entity)).ToList();
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
	}
}
