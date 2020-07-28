using System;

namespace BLL.Core.Models.Person
{
	public class RandomPersonFilter
	{
		public byte? MinAge { get; set; }
		public byte? MaxAge { get; set; }
		public Guid EventUid { get; set; }
		public long? CityId { get; set; }
	}
}
