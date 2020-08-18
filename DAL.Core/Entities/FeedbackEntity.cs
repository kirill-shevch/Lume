using System;
using System.Collections.Generic;

namespace DAL.Core.Entities
{
	public class FeedbackEntity
	{
		public const string TableName = "Feedback";

		public long FeedbackId { get; set; }
		public Guid FeedbackUid { get; set; }
		public string Text { get; set; }
		public string OperatingSystem { get; set; }
		public string PhoneModel { get; set; }
		public string ApplicationVersion { get; set; }
		public DateTime FeedbackTime { get; set; }
		public long PersonId { get; set; }
		public PersonEntity Person { get; set; }
		public IEnumerable<FeedbackImageContentEntity> FeedbackImageContentEntities { get; set; }
	}
}
