using System;
using System.Collections.Generic;

namespace DAL.Core.Entities
{
	public class ChatMessageEntity
	{
		public const string TableName = "ChatMessage";

		public long ChatMessageId { get; set; }
		public Guid ChatMessageUid { get; set; }
		public string Content { get; set; }
		public DateTime? MessageTime { get; set; }
		public long ChatId { get; set; }
		public IEnumerable<ChatImageContentEntity> ChatImageContentEntities { get; set; }
	}
}