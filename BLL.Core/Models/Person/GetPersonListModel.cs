using System;

namespace BLL.Core.Models.Person
{
	public class GetPersonListModel
	{
		public Guid PersonUid { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public string Filter { get; set; }
	}
}