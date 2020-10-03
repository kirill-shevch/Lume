using System;
using System.Collections.Generic;

namespace DAL.Core.Entities
{
	public class PromoRewardRequestEntity
	{
		public const string TableName = "PromoRewardRequest";
		public long PromoRewardRequestId { get; set; }
		public long EventId { get; set; }
		public Guid PromoRewardRequestUid { get; set; }
		public string AccountingNumber { get; set; }
		public DateTime PromoRewardRequestTime { get; set; }
		public EventEntity Event { get; set; }
		public IEnumerable<PromoRewardRequestImageContentEntity> Images { get; set; }
	}
}
