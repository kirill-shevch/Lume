namespace DAL.Core.Entities
{
	public class PersonToChatEntity
	{
		public const string TableName = "PersonToChat";
		public long PersonId { get; set; }
		public long ChatId { get; set; }
		public ChatEntity Chat{ get; set; }
		public PersonEntity Person { get; set; }

	}
}