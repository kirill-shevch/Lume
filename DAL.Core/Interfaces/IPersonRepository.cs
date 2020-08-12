using DAL.Core.Entities;
using DAL.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Core.Interfaces
{
	public interface IPersonRepository
	{
		Task<PersonEntity> GetPerson(Guid uid, CancellationToken cancellationToken = default);
		Task CreatePerson(Guid personUid, CancellationToken cancellationToken = default);
		Task UpdatePerson(PersonEntity person, CancellationToken cancellationToken = default);
		Task AddFriendToPerson(Guid personUid, Guid friendUid, bool isApproved, CancellationToken cancellationToken = default);
		Task RemoveFriendFromPerson(Guid personUid, Guid friendUid, CancellationToken cancellationToken = default);
		Task<bool> CheckPersonExistence(Guid personUid, CancellationToken cancellationToken = default);
		Task<bool> CheckPersonExistence(Guid personUid, string login, CancellationToken cancellationToken = default);
		Task<bool> CheckPersonFriendExistence(Guid personUid, Guid friendUid, CancellationToken cancellationToken = default);
		Task<IEnumerable<PersonEntity>> GetPersonListByPage(Guid personUid, RepositoryGetPersonListFilter filter, CancellationToken cancellationToken = default);
		Task<List<PersonEntity>> GetAllPersonFriends(Guid personUid, CancellationToken cancellationToken = default);
		Task<PersonEntity> GetRandomPerson(RepositoryRandomPersonFilter filter, long personId);
		Task AddPersonSwipeHistoryRecord(PersonSwipeHistoryEntity entity);
		Task RemovePersonImage(PersonImageContentEntity entity);
		Task ConfirmFriend(Guid uid, Guid friendGuid);
	}
}
