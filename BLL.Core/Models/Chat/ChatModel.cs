using System;
using System.Collections.Generic;

namespace BLL.Core.Models.Chat
{
	public class ChatModel
	{
		public Guid ChatUid { get; set; }
		public bool IsGroupChat { get; set; }
		public string ChatName { get; set; }
		public List<ChatMessageModel> Messages { get; set; }
		public Guid? PersonUid { get; set; }
		public Guid? EventUid { get; set; }
		public Guid? PersonImageUid { get; set; }
		public Guid? EventImageUid { get; set; }
		public int UnreadMessagesCount { get; set; }
		public bool IsMuted { get; set; }
	}
}
