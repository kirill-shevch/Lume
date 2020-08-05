using System;

namespace BLL.Core.Models.Person
{
	public class UpdatePersonModel
	{
		public Guid PersonUid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Login { get; set; }
		public byte? Age { get; set; }
		public long? CityId { get; set; }
		public string Token { get; set; }
	}
}