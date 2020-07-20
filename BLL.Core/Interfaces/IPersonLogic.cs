using BLL.Core.Models.Person;
using System;
using System.Threading.Tasks;

namespace BLL.Core.Interfaces
{
	public interface IPersonLogic
	{
		Task CreatePerson(Guid personUid);
		Task<PersonModel> GetPerson(Guid personUid);
		Task UpdatePerson(UpdatePersonModel updatePersonModel);
		Task<bool> IsPersonFilledUp(Guid personUid);
	}
}