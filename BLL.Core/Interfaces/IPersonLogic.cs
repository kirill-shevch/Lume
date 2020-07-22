using BLL.Core.Models.Person;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Core.Interfaces
{
	public interface IPersonLogic
	{
		Task CreatePerson(Guid personUid);
		Task<PersonModel> GetPerson(Guid personUid);
		Task UpdatePerson(UpdatePersonModel updatePersonModel);
		Task AddFriendToPerson(Guid personUid, Guid friendUid);
		Task RemoveFriendFromPerson(Guid personUid, Guid friendUid);
		Task<bool> IsPersonFilledUp(Guid personUid);
		Task<IEnumerable<PersonModel>> GetPersonListByPage(int pageNumber, int pageSize, string filter = null);
	}
}