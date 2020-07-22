namespace DAL.Core.Entities
{
	public class ParticipantStatusEntity
	{
		public const string TableName = "ParticipantStatus";
		public long ParticipantStatusId { get; set; }
		public string ParticipantStatusName { get; set; }
	}
}