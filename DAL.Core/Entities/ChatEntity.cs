using System.Collections.Generic;

namespace DAL.Core.Entities
{
	public class ChatEntity
	{
		public const string TableName = "Chat";

		public long ChatId { get; set; }
		public string Name { get; set; }
		public IEnumerable<ChatMessageEntity> ChatMessageEntities { get; set; }
		public IEnumerable<PersonToChatEntity> PersonList { get; set; }
	}
}