using System;

namespace BLL.Core.Models.Chat
{
	public class ChatListModel
	{
		public Guid ChatUid { get; set; }
		public bool IsGroupChat { get; set; }
		public string Name { get; set; }
		public ChatMessageModel LastMessage { get; set; }
		public Guid? PersonImageUid { get; set; }
		public Guid? EventImageUid { get; set; }
		public int UnreadMessagesCount { get; set; }
	}
}