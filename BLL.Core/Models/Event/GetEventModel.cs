using BLL.Core.Models.Person;
using Constants;
using System;
using System.Collections.Generic;

namespace BLL.Core.Models.Event
{
	public class GetEventModel
	{
		public Guid EventUid { get; set; }
		public string Name { get; set; }
		public byte? MinAge { get; set; }
		public byte? MaxAge { get; set; }
		public double XCoordinate { get; set; }
		public double YCoordinate { get; set; }
		public string Description { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public EventStatus Status { get; set; }
		public EventType Type { get; set; }
		public Guid? EventImageContentUid { get; set; }
		public Guid? ChatUid { get; set; }
		public PersonModel Administrator { get; set; }
		public IEnumerable<PersonEventModel> Participants { get; set; }
	}
}
