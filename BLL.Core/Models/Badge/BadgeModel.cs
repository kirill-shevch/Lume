using Constants;
using System;

namespace BLL.Core.Models.Badge
{
	public class BadgeModel
	{
		public Guid BadgeImageUid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Received { get; set; }
		public BadgeNames BadgeName { get; set; }
	}
}
