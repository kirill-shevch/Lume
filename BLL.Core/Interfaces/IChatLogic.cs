using BLL.Core.Models.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Core.Interfaces
{
	public interface IChatLogic
	{
		Task<ChatModel> GetChat(Guid chatUid);
		Task<ChatModel> GetPersonChat(Guid uid, Guid personUid);
		Task<List<ChatListModel>> GetPersonChatList(Guid personUid);
		Task<ChatMessageModel> AddChatMessage(AddMessageModel request, Guid personUid);
	}
}