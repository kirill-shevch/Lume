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
		Task<PersonModel> UpdatePerson(UpdatePersonModel updatePersonModel, Guid personUid);
		Task AddFriendToPerson(Guid personUid, Guid friendUid);
		Task RemoveFriendFromPerson(Guid personUid, Guid friendUid);
		Task<bool> IsPersonFilledUp(Guid personUid);
		Task<IEnumerable<PersonModel>> GetPersonListByPage(Guid personUid, GetPersonListFilter model);
		Task<bool> CheckFriendship(Guid personUid, Guid friendUid);
		Task<List<PersonModel>> GetAllPersonFriends(Guid personUid);
		Task<PersonModel> GetRandomPerson(RandomPersonFilter randomPersonFilter, Guid uid);
		Task AddPersonSwipeHistory(Guid eventUid, Guid personUid);
		Task ConfirmFriend(Guid uid, Guid friendGuid);
		Task<PersonNotificationsModel> GetPersonNotifications(Guid uid);
	}
}