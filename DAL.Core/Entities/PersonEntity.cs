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
		public string Login { get; set; }
		public byte? Age { get; set; }
		public long? PersonImageContentId { get; set; }
		public long? CityId { get; set; }
		public string Token { get; set; }
		public PersonImageContentEntity PersonImageContentEntity { get; set; }
		public IEnumerable<PersonFriendListEntity> FriendList { get; set; }
		public IEnumerable<PersonSwipeHistoryEntity> SwipeHistory { get; set; }
		public IEnumerable<PersonToEventEntity> Events { get; set; }
		public IEnumerable<FeedbackEntity> Feedbacks { get; set; }
		public CityEntity City { get; set; }
	}
}
