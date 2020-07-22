namespace DAL.Core.Entities
{
	public class PersonToEventEntity
	{
		public const string TableName = "PersonToEvent";
		public long PersonId { get; set; }
		public long EventId { get; set; }
		public long? ParticipantStatusId { get; set; }
		public PersonEntity Person { get; set; }
		public EventEntity Event { get; set; }
		public ParticipantStatusEntity ParticipantStatus { get; set; }
	}
}