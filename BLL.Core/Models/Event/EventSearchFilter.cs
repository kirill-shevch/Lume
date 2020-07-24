using Constants;
using System;

namespace BLL.Core.Models.Event
{
	public class EventSearchFilter
	{
		public string Name { get; set; }
		public byte? MinAge { get; set; }
		public byte? MaxAge { get; set; }
		public string Description { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public EventType? Type{ get; set; }
		public EventStatus? Status { get; set; }
	}
}
