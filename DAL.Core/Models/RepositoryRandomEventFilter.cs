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
		public List<long> IgnoringEventList { get; set; }
	}
}