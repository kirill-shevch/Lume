using DAL.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Core.Interfaces
{
	public interface IChatRepository
	{
		Task<ChatEntity> GetChat(Guid uid, CancellationToken cancellationToken = default);
		Task<List<ChatMessageEntity>> GetChatMessages(long chatId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
		Task<bool> CheckChatMessageExistence(Guid chatMessageUid, CancellationToken cancellationToken = default);
		Task<bool> CheckChatExistence(Guid chatUid, CancellationToken cancellationToken = default);
		Task<ChatEntity> GetPersonChat(Guid uid, Guid personUid);
		Task<Guid> CreatePersonalChat(Guid firstPersonUid, Guid secondPersonUid);
		Task<List<PersonToChatEntity>> GetPersonChats(Guid uid);
		Task AddChatMessage(ChatMessageEntity chatMessageEntity);
		Task<string> GetPersonalChatName(long chatId, long personId);
	}
}