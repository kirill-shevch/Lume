using System;

namespace DAL.Core.Entities
{
	public class PromoRewardRequestImageContentEntity
	{
		public const string TableName = "PromoRewardRequestImageContent";
		public long PromoRewardRequestImageContentId { get; set; }
		public long PromoRewardRequestId { get; set; }
		public Guid PromoRewardRequestImageContentUid { get; set; }
	}
}
