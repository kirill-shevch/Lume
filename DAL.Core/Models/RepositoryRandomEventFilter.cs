using Constants;
using System;
using System.Collections.Generic;

namespace DAL.Core.Models
{
	public class RepositoryRandomEventFilter
	{
		public int Age { get; set; }
		public double? PersonXCoordinate { get; set; }
		public double? PersonYCoordinate { get; set; }
		public double? Distance { get; set; }
		public Guid	PersonUid { get; set; }
		public long? CityId { get; set; }
		public bool? IsOpenForInvitations { get; set; }
		public bool? IsOnline { get; set; }
		public List<long> IgnoringEventList { get; set; }
		public List<long> EventTypes { get; set; }
	}
}