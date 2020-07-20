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
		public PersonLogic(IPersonRepository personRepository)
		{
			_personRepository = personRepository;
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
			var model = PersonEntityToModel(entity);
			model.Friends = entity.FriendList.Select(x => PersonEntityToModel(x.Friend)).ToList();
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
