using DAL.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Core.Interfaces
{
	public interface ICoreRepository
	{
		Task<ChatEntity> GetChat(int id, CancellationToken cancellationToken = default);
		Task<PersonEntity> GetPerson(Guid uid, CancellationToken cancellationToken = default);
		Task<EventEntity> GetEvent(int id, CancellationToken cancellationToken = default);
		Task CreatePerson(Guid personUid, CancellationToken cancellationToken = default);
		Task UpdatePerson(PersonEntity person, CancellationToken cancellationToken = default);
		Task<bool> CheckPersonExistence(Guid personUid, CancellationToken cancellationToken = default);
		Task<bool> CheckEventExistence(Guid eventUid, CancellationToken cancellationToken = default);
		Task<bool> CheckChatMessageExistence(Guid chatMessageUid, CancellationToken cancellationToken = default);
	}
}