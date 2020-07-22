using System;
using System.Collections.Generic;

namespace BLL.Core.Models.Chat
{
	public class ChatMessageModel
	{
		public Guid MessageUid { get; set; }
		public string MessageContent { get; set; }
		public List<Guid> Images { get; set; }
		public string PersonName { get; set; }
		public Guid PersonUid { get; set; }
		public Guid? PersonImageUid { get; set; }
	}
}
