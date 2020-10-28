namespace DAL.Core.Entities
{
	public class PersonalChatTuningEntity
	{
		public const string TableName = "PersonalChatTuning";
		public long ChatId { get; set; }
		public long PersonId { get; set; }
		public bool IsMuted { get; set; }
		public ChatEntity Chat { get; set; }
		public PersonEntity Person { get; set; }
	}
}
