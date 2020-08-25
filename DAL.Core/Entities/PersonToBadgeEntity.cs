namespace DAL.Core.Entities
{
	public class PersonToBadgeEntity
	{
		public const string TableName = "PersonToBadge";
		public long PersonId { get; set; }
		public long BadgeId { get; set; }
		public bool IsViewed { get; set; }
		public PersonEntity Person { get; set; }
		public BadgeEntity Badge { get; set; }
	}
}
