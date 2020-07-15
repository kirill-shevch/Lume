namespace DAL.Core.Entities
{
	public class EventStatusEntity
	{
		public const string TableName = "EventStatus";
		public long EventStatusId { get; set; }
		public string EventStatusName { get; set; }
	}
}