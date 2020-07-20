using System;

namespace BLL.Core.Models.Person
{
	public class UpdatePersonModel
	{
		public Guid PersonUid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public byte? Age { get; set; }
	}
}