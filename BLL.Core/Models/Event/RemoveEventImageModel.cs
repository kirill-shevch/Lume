using System;

namespace BLL.Core.Models.Event
{
	public class RemoveEventImageModel
	{
		public Guid EventUid { get; set; }
		public Guid ImageUid { get; set; }
	}
}
