namespace DAL.Core.Entities
{
	public class PersonFriendListEntity
	{
		public const string TableName = "PersonFriendList";
		public long PersonId { get; set; }
		public long FriendId { get; set; }
		public PersonEntity Person { get; set; }
		public PersonEntity Friend { get; set; }
	}
}
