using BLL.Core.Interfaces;
using BLL.Core.Models;
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
			return new PersonModel
			{
				PersonUid = entity.PersonUid,
				Name = entity.Name,
				Description = entity.Description,
				Age = entity.Age,
				ImageContentUid = entity.PersonImageContentEntity?.PersonImageContentUid,
				Friends = entity.FriendList.Select(x => new PersonModel 
				{ 
					PersonUid = x.Friend.PersonUid,
					Name = x.Friend.Name,
					Description = x.Friend.Description,
					Age = x.Friend.Age,
					ImageContentUid = x.Friend.PersonImageContentEntity?.PersonImageContentUid,
				}).ToList()
			};
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
	}
}