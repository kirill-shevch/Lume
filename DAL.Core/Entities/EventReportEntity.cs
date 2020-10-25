using System;

namespace DAL.Core.Entities
{
	public class EventReportEntity
	{
		public const string TableName = "EventReport";
		public long EventReportId { get; set; }
		public Guid EventReportUid { get; set; }
		public string Text { get; set; }
		public DateTime CreationTime { get; set; }
		public long EventId { get; set; }
		public EventEntity Event { get; set; }
		public bool IsProcessed { get; set; }
	}
}
