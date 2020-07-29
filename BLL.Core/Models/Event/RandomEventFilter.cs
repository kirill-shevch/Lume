using System;
using System.Collections.Generic;

namespace BLL.Core.Models.Event
{
	public class RandomEventFilter
	{
		public double? PersonXCoordinate { get; set; }
		public double? PersonYCoordinate { get; set; }
		public double? Distance { get; set; }
		public long? CityId { get; set; }
	}
}