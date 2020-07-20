using BLL.Core.Models.Event;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Core.Interfaces
{
	public interface IEventLogic
	{
		Task<Guid> AddEvent(AddEventModel addEventModel, Guid personUid);
		Task<GetEventModel> GetEvent(Guid eventUid);
		Task<List<GetEventListModel>> GetEventList(Guid personUid);
		Task UpdateEvent(UpdateEventModel updateEventModel);
	}
}