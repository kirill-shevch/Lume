using System;

namespace DAL.Core.Entities
{
	public class FeedbackImageContentEntity
	{
		public const string TableName = "FeedbackImageContent";

		public long FeedbackImageContentId { get; set; }
		public Guid FeedbackImageContentUid { get; set; }
		public long FeedbackId { get; set; }
	}
}
