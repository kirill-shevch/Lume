namespace BLL.Core.Models.Person
{
	public class PersonNotificationsModel
	{
		public int NewFriendsCount { get; set; }
		public int NewEventInvitationsCount { get; set; }
		public bool AnyNewChatMessages { get; set; }
	}
}
