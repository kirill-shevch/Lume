using Constants;
using System;

namespace BLL.Core.Models.Event
{
	public class UpdateEventModel
	{
		public Guid EventUid { get; set; }
		public string Name { get; set; }
		public byte? MinAge { get; set; }
		public byte? MaxAge { get; set; }
		public double? XCoordinate { get; set; }
		public double? YCoordinate { get; set; }
		public string Description { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public bool? IsOpenForInvitations { get; set; }
		public EventStatus? Status { get; set; }
		public EventType? Type { get; set; }
		public long? CityId { get; set; }
	}
}