using Constants;
using System;

namespace BLL.Core.Models.Event
{
	public class GetEventListModel
	{
		public Guid EventUid { get; set; }
		public string Name { get; set; }
		public double XCoordinate { get; set; }
		public double YCoordinate { get; set; }
		public string Description { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public EventStatus Status { get; set; }
		public EventType Type { get; set; }
		public Guid? EventImageContentUid { get; set; }
		public bool IsAdministrator { get; set; }
		public ParticipantStatus ParticipantStatus { get; set; }
		public bool AnyPersonWaitingForApprove { get; set; }
	}
}