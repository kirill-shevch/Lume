namespace DAL.Core.Entities
{
	public class PersonSwipeHistoryEntity
	{
		public const string TableName = "PersonSwipeHistory";
		public long PersonId { get; set; }
		public long EventId { get; set; }
		public PersonEntity Person { get; set; }
		public EventEntity Event { get; set; }
	}
}
