using System;
using System.Collections.Generic;

namespace BLL.Core.Models.Event
{
	public class PromoRewardRequestModel
	{
		public Guid EventUid { get; set; }
		public string AccountingNumber { get; set; }
		public List<byte[]> Images { get; set; }
	}
}
