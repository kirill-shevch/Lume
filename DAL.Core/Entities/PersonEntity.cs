using System;
using System.Collections.Generic;

namespace DAL.Core.Entities
{
	public class PersonEntity
	{
		public const string TableName = "Person";
		public long PersonId { get; set; }
		public Guid PersonUid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public byte? Age { get; set; }
		public long? PersonImageContentId { get; set; }
		public PersonImageContentEntity PersonImageContentEntity { get; set; }
		public IEnumerable<PersonFriendListEntity> FriendList { get; set; }
		public IEnumerable<PersonToChatEntity> ChatList { get; set; }
		public IEnumerable<PersonToEventEntity> Events { get; set; }
	}
}
