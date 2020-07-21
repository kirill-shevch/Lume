using BLL.Core.Models.Chat;
using System;
using System.Threading.Tasks;

namespace BLL.Core.Interfaces
{
	public interface IChatLogic
	{
		Task<ChatModel> GetChat(Guid chatUid);
	}
}
