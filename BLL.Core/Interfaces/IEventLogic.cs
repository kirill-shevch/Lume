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
		Task<GetEventModel> UpdateEvent(UpdateEventModel updateEventModel);
		Task<GetEventModel> AddParticipant(EventParticipantModel eventParticipantModel, Guid personUid);
		Task UpdateParticipant(EventParticipantModel eventParticipantModel);
		Task RemoveParticipant(Guid personUid, Guid eventUid);
		Task<GetEventModel> GetRandomEvent(RandomEventFilter filter, Guid personUid);
		Task<List<GetEventListModel>> SearchForEvent(EventSearchFilter filter);
		Task AddEventSwipeHistory(Guid personUid, Guid eventUid);
		Task AddPromoRewardRequest(PromoRewardRequestModel request);
	}
}