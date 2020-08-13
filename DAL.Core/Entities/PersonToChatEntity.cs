namespace DAL.Core.Entities
{
	public class PersonToChatEntity
	{
		public const string TableName = "PersonToChat";
		public long FirstPersonId { get; set; }
		public long SecondPersonId { get; set; }
		public long ChatId { get; set; }
		public long? LastReadChatMessageId { get; set; }
		public ChatEntity Chat{ get; set; }
		public PersonEntity FirstPerson { get; set; }
		public PersonEntity SecondPerson { get; set; }
	}
}