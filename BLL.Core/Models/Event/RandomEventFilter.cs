using Constants;
using System.Collections.Generic;

namespace BLL.Core.Models.Event
{
	public class RandomEventFilter
	{
		public double? PersonXCoordinate { get; set; }
		public double? PersonYCoordinate { get; set; }
		public double? Distance { get; set; }
		public long? CityId { get; set; }
		public List<EventType> EventTypes { get; set; }
		public bool? IsOpenForInvitations { get; set; }
		public bool? IsOnline { get; set; }
	}
}