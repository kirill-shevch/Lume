namespace DAL.Core.Entities
{
	public class EventImageContentEntity
	{
		public const string TableName = "EventImageContent";
		public long EventImageContentId { get; set; }
		public string ContentHash { get; set; }
		public byte[] Content { get; set; }
	}
}