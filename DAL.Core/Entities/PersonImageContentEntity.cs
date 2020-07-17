using System;

namespace DAL.Core.Entities
{
	public class PersonImageContentEntity
	{
		public const string TableName = "PersonImageContent";
		public long PersonImageContentId { get; set; }
		public Guid PersonImageContentUid { get; set; }
		public string ContentHash { get; set; }
		public byte[] Content { get; set; }
	}
}