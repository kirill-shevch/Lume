using System;
using System.Collections.Generic;

namespace BLL.Core.Models.Chat
{
	public class AddMessageModel
	{
		public Guid ChatUid { get; set; }
		public string Content { get; set; }
		public List<byte[]> Images { get; set; }
	}
}