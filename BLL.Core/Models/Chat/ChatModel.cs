using System;
using System.Collections.Generic;

namespace BLL.Core.Models.Chat
{
	public class ChatModel
	{
		public Guid ChatUid { get; set; }
		public bool IsGroupChat { get; set; }
		public List<ChatMessageModel> Messages { get; set; }
	}
}
