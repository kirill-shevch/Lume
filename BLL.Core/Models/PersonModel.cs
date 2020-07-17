using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Core.Models
{
	public class PersonModel
	{
		public Guid PersonUid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public byte? Age { get; set; }
		public byte[] ImageContent { get; set; }
		public List<PersonModel> Friends{ get; set; }
	}
}