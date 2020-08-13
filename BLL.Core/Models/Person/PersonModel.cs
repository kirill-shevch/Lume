using System;
using System.Collections.Generic;

namespace BLL.Core.Models.Person
{
	public class PersonModel
	{
		public Guid PersonUid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Login { get; set; }
		public byte? Age { get; set; }
		public Guid? ImageContentUid{ get; set; }
		public bool? IsFriend { get; set; }
		public bool? FriendshipApprovalRequired { get; set; }
		public List<PersonModel> Friends { get; set; }
		public long? CityId { get; set; }
		public string CityName { get; set; }
	}
}