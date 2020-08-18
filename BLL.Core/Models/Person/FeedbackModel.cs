using System.Collections.Generic;

namespace BLL.Core.Models.Person
{
	public class FeedbackModel
	{
		public string Text { get; set; }
		public string OperatingSystem { get; set; }
		public string PhoneModel { get; set; }
		public string ApplicationVersion { get; set; }
		public List<byte[]> Images { get; set; }
	}
}
