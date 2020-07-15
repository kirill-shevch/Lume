namespace DAL.Core.Entities
{
	public class EventTypeEntity
	{
		public const string TableName = "EventType";
		public long EventTypeId { get; set; }
		public string EventTypeName { get; set; }
	}
}