using System;
using System.Collections.Generic;

namespace DAL.Core.Entities
{
	public class ChatEntity
	{
		public const string TableName = "Chat";

		public long ChatId { get; set; }
		public Guid ChatUid { get; set; }
		public bool? IsGroupChat { get; set; }
		public IEnumerable<ChatMessageEntity> ChatMessageEntities { get; set; }
		public IEnumerable<PersonToChatEntity> PersonList { get; set; }
		public IEnumerable<PersonalChatTuningEntity> PersonalSettings { get; set; }
	}
}