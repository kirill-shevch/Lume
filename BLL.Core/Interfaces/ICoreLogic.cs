using BLL.Core.Models;
using BLL.Core.Models.Event;
using BLL.Core.Models.Person;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Core.Interfaces
{
	public interface ICoreLogic
	{
		Task CreatePerson(Guid personUid);
		Task<PersonModel> GetPerson(Guid personUid);
		Task UpdatePerson(UpdatePersonModel updatePersonModel);
		Task<bool> IsPersonFilledUp(Guid personUid);

		Task<Guid> AddEvent(AddEventModel addEventModel, Guid personUid);
		Task<GetEventModel> GetEvent(Guid eventUid);
		Task<List<GetEventListModel>> GetEventList(Guid personUid);
		Task UpdateEvent(UpdateEventModel updateEventModel);
	}
}