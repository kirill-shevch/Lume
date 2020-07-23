using System;

namespace DAL.Core.Entities
{
	public class EventImageContentEntity
	{
		public const string TableName = "EventImageContent";
		public long EventImageContentId { get; set; }
		public Guid EventImageContentUid { get; set; }
		public byte[] Content { get; set; }
		public bool? IsPrimary { get; set; }
		public long EventId { get; set; }
	}
}