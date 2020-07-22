using System;
using System.Collections.Generic;

namespace LumeWebApp.Requests.Chat
{
	public class AddChatRequest
	{
		public List<Guid> Participants { get; set; }
	}
}
