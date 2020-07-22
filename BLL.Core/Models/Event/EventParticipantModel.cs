using Constants;
using System;

namespace BLL.Core.Models.Event
{
	public class EventParticipantModel
	{
		public Guid PersonUid { get; set; }
		public Guid EventUid { get; set; }
		public ParticipantStatus ParticipantStatus { get; set; }
	}
}
