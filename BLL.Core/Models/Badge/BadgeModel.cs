using System;

namespace BLL.Core.Models.Badge
{
	public class BadgeModel
	{
		public Guid BadgeUid { get; set; }
		public string Name { get; set; }
		public bool IsViewed { get; set; }
	}
}
