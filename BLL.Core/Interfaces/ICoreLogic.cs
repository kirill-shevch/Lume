using BLL.Core.Models;
using System;
using System.Threading.Tasks;

namespace BLL.Core.Interfaces
{
	public interface ICoreLogic
	{
		Task CreatePerson(Guid personUid);
		Task<PersonModel> GetPerson(Guid personUid);
		Task UpdatePerson(UpdatePersonModel updatePersonModel);
		Task<bool> IsPersonFilledUp(Guid personUid);
	}
}