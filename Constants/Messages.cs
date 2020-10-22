using System.Collections.Generic;

namespace Constants
{
	public enum MessageTitles
	{
		UpdateSuccess,
		FriendAdded,
		FriendConfirmed,
		FriendRemoved,
		ParticipantCreated,
		RandomEventAccepted,
		RandomEventRejected,
		RandomPersonAccepted,
		RandomPersonRejected,
		PushNotificationMessage,
		AddParticipantNotificationMessage,
		FeedbackAdded,
		PromoRewardRequestAdded,
		EventImageRemoved,
		ParticipantWaitingForApproval,
		ParticipantJoinedTheEvent
	}

	public static class Messages
	{
		private static Dictionary<(MessageTitles, string), string> MessageContent = new Dictionary<(MessageTitles, string), string>
		{
			[(MessageTitles.UpdateSuccess, "en-US")] = "Update completed!",
			[(MessageTitles.UpdateSuccess, "ru-RU")] = "Обновление завершено",
			[(MessageTitles.FriendAdded, "en-US")] = "Friend has been succesfully added!",
			[(MessageTitles.FriendAdded, "ru-RU")] = "Друг успешно добавлен!",
			[(MessageTitles.FriendConfirmed, "en-US")] = "Friend has been succesfully confirmed!",
			[(MessageTitles.FriendConfirmed, "ru-RU")] = "Друг успешно подтверждён!",
			[(MessageTitles.FriendRemoved, "en-US")] = "Friend has been succesfully removed!",
			[(MessageTitles.FriendRemoved, "ru-RU")] = "Друг успешно удалён!",
			[(MessageTitles.ParticipantCreated, "en-US")] = "Participant has been succesfully created!",
			[(MessageTitles.ParticipantCreated, "ru-RU")] = "Участник события успешно создан!",
			[(MessageTitles.RandomEventAccepted, "en-US")] = "Random event has been succesfully accepted!",
			[(MessageTitles.RandomEventAccepted, "ru-RU")] = "Случайное событие принятно!",
			[(MessageTitles.RandomEventRejected, "en-US")] = "Random event has been succesfully rejected!",
			[(MessageTitles.RandomEventRejected, "ru-RU")] = "Случайное событие отклонено!",
			[(MessageTitles.RandomPersonAccepted, "en-US")] = "Random person has been succesfully accepted!",
			[(MessageTitles.RandomPersonAccepted, "ru-RU")] = "Случайный пользователь принят!",
			[(MessageTitles.RandomPersonRejected, "en-US")] = "Random person has been succesfully rejected!",
			[(MessageTitles.RandomPersonRejected, "ru-RU")] = "Случайный пользователь отклонён!",
			[(MessageTitles.PushNotificationMessage, "en-US")] = "Authorization code: {0}",
			[(MessageTitles.PushNotificationMessage, "ru-RU")] = "Код авторизации: {0}",
			[(MessageTitles.AddParticipantNotificationMessage, "en-US")] = @"You were invited to the event ""{0}""",
			[(MessageTitles.AddParticipantNotificationMessage, "ru-RU")] = @"Вас пригласили на событие ""{0}""",
			[(MessageTitles.FeedbackAdded, "en-US")] = "Feedback added!",
			[(MessageTitles.FeedbackAdded, "ru-RU")] = "Отзыв добавлен!",
			[(MessageTitles.PromoRewardRequestAdded, "en-US")] = "Reward request added!",
			[(MessageTitles.PromoRewardRequestAdded, "ru-RU")] = "Запрос на полчение вознаграждения добавлен!",
			[(MessageTitles.EventImageRemoved, "en-US")] = "Event image removed!",
			[(MessageTitles.EventImageRemoved, "ru-RU")] = "Изображение у события удалено!",
			[(MessageTitles.ParticipantWaitingForApproval, "en-US")] = @"New member wants to join the event {0}",
			[(MessageTitles.ParticipantWaitingForApproval, "ru-RU")] = @"Новый участник хочет присоединиться к событию {0} ",
			[(MessageTitles.ParticipantJoinedTheEvent, "en-US")] = @"New member joined event {0}",
			[(MessageTitles.ParticipantJoinedTheEvent, "ru-RU")] = @"Новый участник присоединился к событию {0}",
		};

		public static string GetMessageJson(MessageTitles title, string culture) => $"{{ \"data\":\"{MessageContent[(title, culture)]}\" }}";
		public static string GetMessage(MessageTitles title, string culture) => MessageContent[(title, culture)];
	}
}
