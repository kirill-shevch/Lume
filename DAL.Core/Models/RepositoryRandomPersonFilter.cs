using System;
using System.Collections.Generic;

namespace DAL.Core.Models
{
	public class RepositoryRandomPersonFilter
	{
		public byte? MinAge { get; set; }
		public byte? MaxAge { get; set; }
		public long EventId { get; set; }
		public long? CityId { get; set; }
		public List<long> IgnoringPersonList { get; set; }
	}
}
