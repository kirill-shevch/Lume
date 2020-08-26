using System;

namespace BLL.Core.Models.Badge
{
	public class BadgeModel
	{
		public Guid BadgeImageUid { get; set; }
		public string Name { get; set; }
		public bool IsViewed { get; set; }
	}
}
