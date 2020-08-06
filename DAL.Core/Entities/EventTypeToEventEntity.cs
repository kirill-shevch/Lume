namespace DAL.Core.Entities
{
	public class EventTypeToEventEntity
	{
		public const string TableName = "EventTypeToEvent";
		public long EventTypeId { get; set; }
		public long EventId { get; set; }
		public EventTypeEntity EventType { get; set; }
		public EventEntity Event { get; set; }
	}
}
