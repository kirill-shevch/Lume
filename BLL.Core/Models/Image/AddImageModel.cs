using System;

namespace BLL.Core.Models.Image
{
	public class AddImageModel
	{
		public Guid Uid { get; set; }
		public byte[] Content { get; set; }
	}
}