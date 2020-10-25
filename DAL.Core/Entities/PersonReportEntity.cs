using System;

namespace DAL.Core.Entities
{
	public class PersonReportEntity
	{
		public const string TableName = "PersonReport";
		public long PersonReportId { get; set; }
		public Guid PersonReportUid { get; set; }
		public string Text { get; set; }
		public DateTime CreationTime { get; set; }
		public long PersonId { get; set; }
		public PersonEntity Person { get; set; }
		public bool IsProcessed { get; set; }
	}
}
