namespace DAL.Core.Entities
{
	public class EventSwipeHistoryEntity
	{
		public const string TableName = "EventSwipeHistory";
		public long EventId { get; set; }
		public long PersonId { get; set; }
		public EventEntity Event { get; set; }
		public PersonEntity Person { get; set; }
	}
}
