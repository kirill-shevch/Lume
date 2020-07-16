using System;

namespace BLL.Core.Models
{
	public class UpdatePersonModel
	{
		public Guid PersonUid { get; set; }
		public string Name { get; set; }
		public string Agenda { get; set; }
		public byte? Age { get; set; }
	}
}